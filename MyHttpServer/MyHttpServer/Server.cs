using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyHttpServer
{
    public class Server
    {
        private readonly HttpListener _server;
        private readonly ServerConfiguration _configuration;
        private bool _isAlive;
        private string _contentType;
        private readonly object _lock = new object();

        public Server()
        {
            _configuration = new ServerConfiguration();
            _server = new HttpListener();
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
            Console.WriteLine("The server started working!");

            var serverTask = ServerProcessAsync();

            Console.WriteLine("Type 'stop' and press 'Enter' to stop the server.");
            while (_isAlive)
            {
                if (Console.ReadLine()?.ToLower() == "stop")
                {
                    Stop();
                    _isAlive = false;
                }
            }

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
                    Console.WriteLine("The server has been stopped.");
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
                var requestUrl = request.Url.AbsolutePath;

                string filePath = requestUrl;
                if (filePath.EndsWith("/"))
                    filePath = _configuration.StaticFilesPath + "/google/index.html";

                if (filePath.StartsWith("/"))
                {
                    filePath = filePath.Trim('/');
                }

                if (!File.Exists(filePath))
                {
                    if (!File.Exists(_configuration.StaticFilesPath + "/google/" + filePath))
                    {
                        filePath = $"{_configuration.StaticFilesPath}/404.html";
                    }
                    else
                    {
                        filePath = _configuration.StaticFilesPath + "/google/" + filePath;
                    }
                }

                string extension = Path.GetExtension(filePath);
                
                ParseExtention(extension);

                HttpListenerResponse response = context.Response;

                response.ContentType = _contentType;

                /* Вывод запросов в консоль
                  
                    Console.WriteLine("\n --------------");
                    Console.WriteLine("request: " + requestUrl);
                    Console.WriteLine("filepath : " + filePath);
                    Console.WriteLine("extention: " + extension);
                    Console.WriteLine("contentType: " + _contentType);
                    Console.WriteLine("-------------- \n");

                */

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
