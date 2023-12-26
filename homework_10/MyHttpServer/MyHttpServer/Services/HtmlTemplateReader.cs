using System.Text.Json;
using TemplatingEngine;
using HtmlAgilityPack;
using MyHttpServer.Model;
using System.Net;
using System.Text;

namespace MyHttpServer.Services
{
    public class HtmlTemplateReader
    {
        private const string _buttonsFilePath = "buttons.json";
        private TemplateParser _templateParser;
        private Buttons _buttons;

        public HtmlTemplateReader()
        {
            GetAccountsFromJson();
            _templateParser = new TemplateParser();
        }

        private void GetAccountsFromJson()
        {
            if (!File.Exists(_buttonsFilePath))
            {
                Console.WriteLine("json file not found!");
                throw new FileNotFoundException("json file not found!");
            }

            using (FileStream file = File.OpenRead(_buttonsFilePath))
            {
                _buttons = JsonSerializer.Deserialize<Buttons>(file);
            }
        }

        public async Task UpdateHtmlPage(HttpListenerContext context, string filePath)
        {
            string response = PrepareHtmlPage(filePath);
            await SendHtml(context, response);
        }

        private string PrepareHtmlPage(string filePath)
        {
            string htmlPage = File.ReadAllText(filePath);
            int count = 0;
            string html = File.ReadAllText(filePath);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var htmlNodes = doc.DocumentNode.SelectNodes("//div[@id='templates']");
            
            if(htmlNodes != null)
            {
                List<string> updatedButtons = AddButtons(htmlNodes);
                foreach (var node in htmlNodes)
                {
                    string newNode = $"<div class=\"buttons\" id=\"templates\">{updatedButtons[count]}</div>";
                    HtmlNode newHtmlNode = HtmlNode.CreateNode(newNode);
                    node.ParentNode.ReplaceChild(newHtmlNode, node);
                    count++;
                }

                htmlPage = doc.DocumentNode.OuterHtml;
            }

            return htmlPage;
        }

        private List<string> AddButtons(HtmlNodeCollection nodes)
        {
            List<string> updatedButtons = new List<string>();
            foreach(var node in nodes)
            {
                updatedButtons.Add(_templateParser.Render(node.InnerText, _buttons)); 
            }

            return updatedButtons;
        }

        private async Task SendHtml(HttpListenerContext context, string html)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(html);
            context.Response.ContentType = "text/html";
            context.Response.ContentLength64 = buffer.Length;

            using Stream output = context.Response.OutputStream;
            await output.WriteAsync(buffer);
            await output.FlushAsync();
            context.Response.Close();
        }
    }
}

