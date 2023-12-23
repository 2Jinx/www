namespace ComputerGames
{
    class Program
    {
        static async Task Main()
        {
            Server server = new Server();       
            await server.Start();
        }
    }
}

