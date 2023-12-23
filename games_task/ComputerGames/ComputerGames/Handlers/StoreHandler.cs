using Npgsql;
using ComputerGames.Model;
using ComputerGames.Services;
using System.Net;
using System.Web;

namespace ComputerGames.Handlers
{
    public class StoreHandler
    {
        private List<Game> _GamesList;
        private string _connectionString;
        private Dictionary<int, string> _tableItems;

        public StoreHandler()
        {
            _GamesList = new List<Game>();
            _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=server;Database=ComputerGames";
            _tableItems = new Dictionary<int, string>()
            {
                { 0, "GAME_NAME"},
                { 1, "GENRE"},
                { 2, "RELEASE_DATE"},
                { 3, "RATING"},
                { 4, "AVAILABLE"},
                { 5, "PRICE"}
            };
        }

        public string Handle(HttpListenerContext context)
        {
            string input = HttpUtility.UrlDecode(new StreamReader(context.Request!.InputStream).ReadToEnd());
            string sqlCommand = "SELECT * FROM GAMES";
            if (input.Count() != 0)
            {
                sqlCommand = ParseFilter(input);
            }
            Console.WriteLine(input);
            Console.WriteLine(sqlCommand);
            GetGamesList(sqlCommand);

            string responseText = new StoreWrapper(_GamesList).CreateStore();
            return responseText;
        }

        private void GetGamesList(string sqlCommand)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(sqlCommand, connection))
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            _GamesList.Add(new Game(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetValue(3),
                                reader.GetInt32(4),
                                reader.GetBoolean(5),
                                reader.GetInt32(6)
                            ));
                        }
                    }
                }
            }
        }

        private string ParseFilter(string filter)
        {
            string sqlCommand = "SELECT * FROM GAMES";
            int id = 0;
            bool flag = true;
            string[] parts = filter.Split('&');
            string[] items = new string[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                string[] pair = parts[i].Split('=');
                items[i] = pair.Length > 1 ? pair[1] : string.Empty;
            }
            if (items[4] == "In stock" || items[4] == "Out of stock")
            {
                if (items[4] == "In stock")
                    items[4] = "true";
                else
                    items[4] = "false";
            }
            else
                items[4] = string.Empty;

            foreach (string item in items)
            {
                Console.WriteLine(item);
            }

            while (id != 6)
            {
                if (!string.IsNullOrEmpty(items[id].ToString()))
                {
                    if (flag)
                    {
                        if(id <= 2)
                        {
                            sqlCommand += $" WHERE {_tableItems[id]} = '{items[id]}'";
                        }
                        if(id == 3 || id == 5)
                        {
                            sqlCommand += $" WHERE {_tableItems[id]} = {int.Parse(items[id])}";
                        }
                        else if (id == 4)
                        {
                            sqlCommand += $" WHERE {_tableItems[id]} = {items[id]}";
                        }
                        flag = false;
                    }
                    else if (!flag)
                    {
                        if (id <= 2)
                        {
                            sqlCommand += $" AND {_tableItems[id]} = '{items[id]}'";
                        }
                        if (id == 3 || id == 5)
                        {
                            sqlCommand += $" AND {_tableItems[id]} = {int.Parse(items[id])}";
                        }
                        else if (id == 4)
                        {
                            sqlCommand += $" AND {_tableItems[id]} = {items[id]}";
                        }
                    }
                    
                }
                id++;
            }

            return sqlCommand;
        }
    }
}

