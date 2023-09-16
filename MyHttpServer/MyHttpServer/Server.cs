using System;
using System.Net;

namespace MyHttpServer
{
    public class Server
    {
        HttpListener server;
        ServerConfiguration configuration;

        public Server()
        {
            server = new HttpListener();
            configuration.Set(server);
            server.Start();
        }
    }
}

