using System.Net;

namespace Handler.MyHttpServer
{
    public interface IHandler
    {
        public async void Handle(HttpListenerContext context) { }
    }
}

