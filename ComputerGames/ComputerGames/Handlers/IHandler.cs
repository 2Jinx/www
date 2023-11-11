using System.Net;

namespace ComputerGames.Handler
{
    public interface IHandler
    {
        public async void Handle(HttpListenerContext context) { }
    }
}