using ComputerGames.Model;

namespace ComputerGames.Services
{
    public class StoreWrapper
    {
        private List<Game> _GamesList;

        public StoreWrapper(List<Game> gamesList)
        {
            _GamesList = gamesList;
        }

        public string CreateStore()
        {
            string html = "<!DOCTYPE html>" +
                          "<html>" +
                          "<head>" +
                            "<title>Store</title>" +
                            "<meta charset=\"UTF-8\">" +
                            "<style>" +
                                "body {" +
                                    "font-family: Arial, Helvetica, sans-serif;" +
                                    "background-color: #333;" +
                                    "justify-content: center;" +
                                    "align-items: center;" +
                                    "display: flex;" +
                                    "flex-direction: column;" +
                                "}" +
                                "img {" +
                                    "max-width: 100px;" +
                                    "max-height: 100px;" +
                                    "border-radius: 5px;" +
                                "}" +
                                "table {" +
                                    "margin: 0 auto;" +
                                    "text-align: center;" +
                                    "color: white;" +
                                    "}" +
                                "th, td {" +
                                    "padding: 8px;" +
                                "}" +
                                "form {" +
                                    "margin-top: 50px;" +
                                    "text-align: center;" +
                                    "padding-bottom: 50px" +
                                "}" +
                                "input {" +
                                    "width: 200px;" +
                                    "padding: 10px;" +
                                    "margin: 5px;" +
                                "}" +
                                "input[type=\"submit\"] {" +
                                    "width: auto;" +
                                    "padding: 10px 20px;" +
                                "}" +
                                ".button {" +
                                    "padding: 15px 30px;" +
                                    "font-size: 16px;" +
                                    "background-color: #3c19ae;" +
                                    "border: none;" +
                                    "border-radius: 5px;" +
                                    "color: white;" +
                                    "text-align: center;" +
                                    "text-decoration: none;" +
                                    "display: inline-block;" +
                                    "transition: background-color 0.3s;" +
                                    "}" +
                                    ".button:hover {" +
                                        "background-color: #3b11c8;" +
                                    "}" +
                            "</style>" +
                          "</head>" +
                          "<body>" +
                            "<form method=\"post\">" +
                                "<input type=\"text\" placeholder=\"Name\" name=\"name\">" +
                                "<input type=\"text\" placeholder=\"Genre\" name=\"genre\">" +
                                "<input type=\"text\" placeholder=\"Release Date\" name=\"release\">" +
                                "<input type=\"text\" placeholder=\"Rating\" name=\"rating\">" +
                                "<input type=\"text\" placeholder=\"Stock\" name=\"stock\">" +
                                "<input type=\"text\" placeholder=\"Price\" name=\"price\">" +
                                "<input type=\"submit\" value=\"Отправить\">" +
                            "</form>" +
                            "<table>" +
                                "<tr>" +
                                    "<th></th>" +
                                    "<th>Name</th>" +
                                    "<th>Genre</th>" +
                                    "<th>Release date</th>" +
                                    "<th>Rating</th>" +
                                    "<th>Stock</th>" +
                                    "<th>Price</th>" +
                                "</tr>";

            foreach(var game in _GamesList)
            {
                html += "<tr>" +
                            $"<td><img src=\"{game.Picture}\"></td>" +
                            $"<td>{game.Name}</td>" +
                            $"<td>{game.Genre}</td>" +
                            $"<td>{game.ReleaseDate}</td>" +
                            $"<td>{game.Rating}</td>" +
                            $"<td>{game.Available}</td>" +
                            $"<td>{game.Price}$</td>" +
                         "</tr>";
            }

            html += "</table>" +
                    "<a href=\"/\" class=\"button\">Главная</a>" +
                    "</body>" +
                    "</html>";

            return html;
        }
    }
}

