using System.Net;
using MyHttpServer.Configuration;
using MyHttpServer.Handler;

namespace MyHttpServer
{
    public class Server
    {
        private readonly HttpListener ? _server;
        private readonly ServerConfiguration ? _configuration;
        private bool _isAlive;
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
                StaticFilesHandler staticHandler = new StaticFilesHandler(_sitePreset, _configuration);
                staticHandler.Handle(context);
            }
        }
    }
}