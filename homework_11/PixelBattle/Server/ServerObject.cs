using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using GameUtils.Paths;

namespace Server
{
    public class ServerObject
    {
        private TcpListener? tcpListener = new TcpListener(IPAddress.Any, 8888); // сервер для прослушивания
        private List<Client>? _clients = new List<Client>(); // все подключения
        private readonly List<SendPoint> _pointsField = new List<SendPoint>();

        protected internal void RemoveConnection(string id)
        {
            Client? client = _clients?.FirstOrDefault(c => c.Id == id); // получаем по id закрытое подключение

            if (client != null) _clients.Remove(client); // и удаляем его из списка подключений
            client?.Close();
        }

        /// <summary>
        /// Прослушивание входящих подключений
        /// </summary>
        /// <returns></returns>
        protected internal async Task ListenAsync()
        {
            try
            {
                tcpListener?.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                    Client client = new Client(tcpClient, this);
                    _clients?.Add(client);
                    await Task.Run(client.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        protected internal async Task BroadcastColoredMessageAsync(AddUser addUser)
        {
            var color = GenerateRandomColor();
            addUser.Color = color;

            foreach (var client in _clients)
            {
                client.Color = color; // Устанавливаем цвет каждому клиенту
                var sb = new StringBuilder();
                sb.Append("AddUser ");
                var message = JsonSerializer.Serialize(addUser);
                sb.Append(message);

                try
                {
                    await client.Writer.WriteLineAsync(sb);
                    await client.Writer.FlushAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected internal async Task BroadcastPointsFieldMessageAsync()
        {
            foreach (var point in _pointsField)
            {
                var sb = new StringBuilder();
                sb.Append("SendPoint ");
                {
                    try
                    {
                        var message = JsonSerializer.Serialize(point);
                        sb.Append(message);
                        await _clients.Last().Writer.WriteLineAsync(sb);
                        await _clients.Last().Writer.FlushAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        protected internal async Task BroadcastPointMessageAsync(SendPoint point)
        {
            var sb = new StringBuilder();
            sb.Append("SendPoint ");
            {
                try
                {
                    sb.Append(JsonSerializer.Serialize(point));
                    foreach (var client in _clients)
                    {
                        await client.Writer.WriteLineAsync(sb);
                        await client.Writer.FlushAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private string GenerateRandomColor()
        {
            var random = new Random();
            var color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            var hexColor = ColorTranslator.ToHtml(color);
            while (_clients.Select(i => i.Color).Contains(hexColor))
                color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            return ColorTranslator.ToHtml(color);
        }

        protected internal async Task SendListAsync()
        {
            var sb = new StringBuilder();
            sb.Append("SendList ");
            sb.Append(JsonSerializer.Serialize(_clients.Select(x => new AddUser(x.UserName!, x.Color!)).ToList()));

            foreach (var client in _clients)
            {
                await client.Writer.WriteLineAsync(sb);
                await client.Writer.FlushAsync();
            }
        }

        protected internal async Task AddPoint(SendPoint point)
        {
            _pointsField.Add(point);
            await BroadcastPointMessageAsync(point);
        }

        /// <summary>
        /// Трансляция сообщения подключенным клиентам
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="id">Id клиента</param>
        /// <returns></returns>
        protected internal async Task BroadcastMessageAsync()
        {
            var usersToJson = JsonSerializer.Serialize(_clients.Select(x => x.UserName).ToList());
            foreach (var client in _clients)
            {
                await client.Writer.WriteLineAsync(usersToJson);
                await client.Writer.FlushAsync();
            }
        }

        /// <summary>
        /// отключение всех клиентов
        /// </summary>
        protected internal void Disconnect()
        {
            foreach (var client in _clients)
            {
                client.Close(); //отключение клиента
            }
            tcpListener?.Stop(); //остановка сервера
        }
    }
}
