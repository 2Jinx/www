using System.Net;
using System.Text;

namespace MyHttpServer
{
    class Program
    {
        static async Task Main()
        {
            Server server = new Server();

            server.Start();
        }
    }
}
