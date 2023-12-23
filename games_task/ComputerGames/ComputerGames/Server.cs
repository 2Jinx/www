using System.Net;
using ComputerGames.Configuration;
using ComputerGames.Handler;

namespace ComputerGames
{
    public class Server
    {
        private readonly HttpListener? _server;
        private readonly ServerConfiguration? _configuration;
        private bool _isAlive;
        private readonly object _lock = new object();

        public Server()
        {
            if (_server == null)
            {
                lock (_lock)
                {
                    if (_server == null)
                    {
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

            if (_isAlive)
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
                new StaticFilesHandler(_configuration).Handle(context);
            }
        }
    }
}