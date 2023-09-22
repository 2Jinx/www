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

            Console.WriteLine("Type 'stop' and press Enter to stop the server.");
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
                Console.WriteLine();
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
                Console.WriteLine(requestUrl);

                string responseText = "", filePath = requestUrl;
                if (filePath.EndsWith("/"))
                    filePath = _configuration.StaticFilesPath + "/google/index.html";

                if (filePath.StartsWith("/"))
                    filePath = filePath.Trim('/');

                if (!File.Exists(filePath))
                {
                    filePath = $"{_configuration.StaticFilesPath}/404.html";
                }

                string extention = filePath.Substring(filePath.LastIndexOf('.'));
                ParseExtention(extention);

                responseText = File.ReadAllText(filePath);

                HttpListenerResponse response = context.Response;
                byte[] buffer = Encoding.UTF8.GetBytes(responseText);

                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.ContentType = _contentType;
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
                    _contentType = "text/stylesheet";
                    break;
                case "jpeg":
                    _contentType = "image/jpeg";
                    break;
                case "png":
                case "svg":
                case "ico":
                    _contentType = "image/" + extention.Substring(1);
                    break;
            }
        }
    }
}
