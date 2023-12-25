using System;
using System.Net;
using MyHttpServer.Configuration;

namespace MyHttpServer.Handler
{
    public class StaticFilesHandler: IHandler
    {
        private readonly ServerConfiguration _configuration;

        public StaticFilesHandler(ServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void Handle(HttpListenerContext context)
        {
            List<string> filesExtentions = new List<string> { ".jpeg", ".html", ".css", ".svg", ".ico", ".png", ".webp" };
            string raw = context.Request.RawUrl!;
            if (raw == "/" || filesExtentions.Contains(Path.GetExtension(raw.Split("/").Last())))
                    await HandleStatic(context);
            else
                new ControllersHandler(_configuration).Handle(context);
        }

        private async Task HandleStatic(HttpListenerContext context)
        {
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string filePath = request.Url!.AbsolutePath;

                if (filePath == "/")
                    filePath = _configuration!.StaticFilesPath + "/" + "index.html";

                if (filePath.StartsWith("/"))
                    filePath = filePath.Trim('/');

                if (!File.Exists(filePath))
                {
                    if (!File.Exists(_configuration!.StaticFilesPath + "/" + filePath))
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        filePath = $"{_configuration.StaticFilesPath}/404.html";
                    }
                    else
                    {
                        filePath = _configuration.StaticFilesPath + "/" + filePath;
                    }
                }

                ParseExtention(Path.GetExtension(filePath), response);

                using (var fileStream = File.OpenRead(filePath))
                {
                    await fileStream.CopyToAsync(response.OutputStream);
                }

                response.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error handling request: {ex.Message}");
                Console.ResetColor();
            }
        }

        private void ParseExtention(string extention, HttpListenerResponse response)
        {
            switch (extention)
            {
                case ".html":
                    response.ContentType = "text/html";
                    break;
                case ".css":
                    response.ContentType = "text/css";
                    break;
                case ".jpeg":
                    response.ContentType = "image/jpeg";
                    break;
                case ".png":
                    response.ContentType = "image/png";
                    break;
                case ".svg":
                    response.ContentType = "image/svg+xml";
                    break;
                case ".ico":
                    response.ContentType = "image/x-icon";
                    break;
            }
        }
    }
}

