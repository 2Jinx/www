using System.Net.Sockets;
using GameUtils.Paths;
using System.Text.Json;

namespace Server
{
    public class Client
    {
        protected internal string Id { get; } = Guid.NewGuid().ToString();
        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }
        public string UserName { get; set; }
        public string? Color { get; set; }

        protected internal TcpClient client;
        protected internal ServerObject _server; // объект сервера

        public Client(TcpClient tcpClient, ServerObject server)
        {
            client = tcpClient;
            _server = server;

            var stream = client.GetStream(); // получаем NetworkStream для взаимодействия с сервером

            Reader = new StreamReader(stream); // создаем StreamReader для чтения данных

            Writer = new StreamWriter(stream); // создаем StreamWriter для отправки данных
        }

        public async Task ProcessAsync()
        {
            try
            {
                UserName = await Reader.ReadLineAsync(); // получаем имя пользователя
                var addUserMessage = new AddUser(UserName, "");
                string? message = $"{UserName} ";
                await _server.BroadcastColoredMessageAsync(addUserMessage);

                await _server.SendListAsync();
                await _server.BroadcastPointsFieldMessageAsync();

                while (true) // в бесконечном цикле получаем сообщения от клиента
                {
                    await Task.Delay(10);

                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message == null) continue;

                        var point = JsonSerializer.Deserialize<SendPoint>(message!);
                        await _server.AddPoint(point!);
                        Console.WriteLine(message);
                    }
                    catch
                    {
                        message = $"{UserName} покинул чат";
                        Console.WriteLine(message);
                        _server.RemoveConnection(Id);
                        await _server.SendListAsync();
                        await _server.BroadcastMessageAsync();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _server.RemoveConnection(Id); // в случае выхода из цикла закрываем ресурсы
            }
        }

        /// <summary>
        /// закрытие подключения
        /// </summary>
        protected internal void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}
