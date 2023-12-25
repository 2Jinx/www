using System.Reflection;
using Npgsql;

namespace MyHttpServer.ORM;

public class MyDataContext
{
    private static string _connectionString = "Host=localhost;Port=5432;Username=jinx;Password=battle;Database=BattleDB";

    public bool Add<T>(T entity)
    {
        string tableName = typeof(T).Name;
        var type = entity?.GetType();
        var properties = type?.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToList();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                var columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
                var values = string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name));
                command.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                foreach (var property in properties)
                {
                    command.Parameters.Add(new NpgsqlParameter("@" + property.Name, property.GetValue(entity)));
                }
                var number = command.ExecuteNonQuery();
                return true;
            }
        }

    }

    public bool Delete<T>(int id)
    {
        var type = typeof(T);
        var tableName = type.Name;

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                var columns = string.Join(" ", typeof(T).GetProperties().Select(p => p.Name));

                string sqlCommand = $"DELETE FROM {tableName} WHERE ID = {id}";
                
                command.CommandText = sqlCommand;
                var number = command.ExecuteNonQuery();

                return true;
            }
        }
    }


    public List<T> Select<T>()
    {
        var tableName = typeof(T).Name;
        var result = new List<T>();
        var sql = $"SELECT * FROM {tableName}";

        using (var connetion = new NpgsqlConnection(_connectionString))
        {
            connetion.Open();
            using (var command = new NpgsqlCommand(sql, connetion))
            {
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance<T>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var prop = obj.GetType().GetProperty(columnName);

                        if (prop != null)
                        {
                            if (!reader.IsDBNull(i))
                            {
                                prop.SetValue(obj, reader.GetValue(i));
                            }
                            else
                            {
                                prop.SetValue(obj, null);
                            }
                        }
                        else
                            break;
                    }
                    result.Add(obj);
                }
            }
        }
        return result;
    }

    public bool Update<T>(T entity)
    {
        var type = entity?.GetType();
        var tableName = typeof(T).Name;
        var id = type?.GetProperty("id");

        var property = type?.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x != type.GetProperty("id"))
            .ToList();

        using (var connetion = new NpgsqlConnection(_connectionString))
        {
            connetion.Open();
            using (var command = connetion.CreateCommand())
            {
                var columns = string.Join(", ", typeof(T).GetProperties().Where(x => x != typeof(T).GetProperty("id")).Select(p => p.Name)).Split(",").ToList();
                var values = string.Join(", ", typeof(T).GetProperties().Where(x => x != typeof(T).GetProperty("id")).Select(p => "@" + p.Name)).Split(",").ToList();
                command.CommandText = $"update {tableName} set ";
                for (int t = 0; t < property?.Count; t++)
                    command.CommandText += $"{columns[t]} = {values[t]},";

                command.CommandText = command.CommandText.Remove(command.CommandText.Length - 1);
                command.CommandText += $" where id = {id.GetValue(entity)}";
                Console.WriteLine(command.CommandText);

                foreach (var prop in property)
                {
                    command.Parameters.Add(new NpgsqlParameter("@" + prop.Name, prop.GetValue(entity)));
                }

                var number = command.ExecuteNonQuery();

                return true;
            }
        }
    }


    public T SelectById<T>(int id)
    {
        var tableName = typeof(T).Name;

        var sql = $"SELECT * FROM {tableName} WHERE ID = {id}";

        using (var connetion = new NpgsqlConnection(_connectionString))
        {
            connetion.Open();
            using (var command = new NpgsqlCommand(sql, connetion))
            {
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var obj = Activator.CreateInstance<T>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var column = reader.GetName(i);
                        var prop = obj.GetType().GetProperty(column);

                        if (prop != null)
                        {
                            if (!reader.IsDBNull(i))
                            {
                                prop.SetValue(obj, reader.GetValue(i));
                            }
                            else
                            {
                                prop.SetValue(obj, null);
                            }
                        }
                        else
                            break;
                    }

                    return obj;
                }
            }
        }
        return default(T);
    }
}