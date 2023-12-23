using System.Text;
using HtmlAgilityPack;
using MyHttpServer.Attributes;

namespace MyHttpServer.Controllers
{
    /// <summary>
    /// Класс для отображения страницы с предметами торговой площадки Steam
    /// </summary>
    [Controller("Orders")]
    public class OrdersController
    {
        [Post("List")]
        public string List()
        {
            string responseText = "<!DOCTYPE html>\r\n<html>\r\n\t<head><meta charset=\"UTF-8\">" +
                "<style>\r\n\t\t\t.market_listing {\r\n\t\t\t\tborder: 1px solid #D2D2D2;\r\n\t\t\t\t" +
                "padding: 50px; color: #8F98A0;\r\n\t\t\t\ttext-align: center;\r\n\t\t\t}\r\n\r\n\t\t\t" +
                ".buttons {\r\n\t\t\t\tdisplay: flex;\r\n\t\t\t\tjustify-content: center;" +
                "\r\n\t\t\t\tgap: 20px;\r\n\t\t\t\tmargin-top: 20px;\r\n\t\t\t}\r\n\r\n\t\t\t" +
                ".green_button {\r\n\t\t\t\tbackground-color: #4CAF50; border-radius: 5px;\r\n\t\t\t\tcolor: #FFFFFF; " +
                "\r\n\t\t\t\tpadding: 10px 20px; \r\n\t\t\t\tborder: none; \r\n\t\t\t\tcursor: pointer; " +
                "\r\n\t\t\t\ttransition: background-color 0.3s ease; \r\n\t\t\t}\r\n\r\n\t\t\t" +
                ".green_button:hover {\r\n\t\t\t\tbackground-color: #45a049;\r\n\t\t\t}\r\n\r\n\t\t</style>" +
                "\r\n\t</head>\r\n\t\t<body style=\"background-color: #15171E\">\r\n\t\t\t<div style=\"display: grid; " +
                "grid-template-columns: repeat(3, 1fr); gap: 50px;\">";

            responseText += GetMarketList("https://steamcommunity.com/market/search?q=", 10);
            responseText += GetMarketList("https://steamcommunity.com/market/search?q=#p1_popular_desc", 5);
            responseText += "</div>\r\n\t<a href=\"/\" style=\"width: 100px; text-align: " +
                "center; place-self: center; display: block; padding: 15px 30px; " +
                "font-family: Arial, Helvetica, sans-serif; background-color: #4CAF50;" +
                "color: white; text-decoration: none; border-radius: 5px; " +
                "margin-top: 20px; margin-left: auto; margin-right: auto;\">Главная</a></body>\r\n</html>";

            return responseText;
        }

        private string GetMarketList(string url, int maxCount = int.MaxValue)
        {
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            var html = web.Load(url);

            var htmlNodes = html.DocumentNode.SelectNodes("//a[@class='market_listing_row_link']");

            if (htmlNodes != null)
            {
                var sb = new StringBuilder();
                foreach (var htmlNode in htmlNodes.Take(maxCount))
                {
                    sb.Append("<div class=\"market_listing\">");
                    sb.Append(htmlNode.InnerHtml);
                    sb.Append("<div class=\"buttons\">\r\n\t\t\t<button class=\"green_button\">Купить</button>" +
                        "\r\n\t\t\t<button class=\"green_button\">Продать</button>\r\n\t\t</div>\t\r\n\t</div>");
                }
                return sb.ToString();
            }

            return string.Empty;
        }

    }
}
