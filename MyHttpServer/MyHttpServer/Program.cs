using System.Net;
using Newtonsoft.Json;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        HttpListener server = new HttpListener();
        // установка адресов прослушки
        if (File.Exists("appsetting.json"))
        {
            string json = File.ReadAllText("appsetting.json");
            bool isAlive = true;
            // Десериализация JSON в словарь
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            // Извлечение порта и адреса из словаря
            if (data.ContainsKey("Port") && data.ContainsKey("Address"))
            {
                string port = data["Port"].ToString();
                string address = data["Address"].ToString();
                server.Prefixes.Add(address + ":" + port + "/connection/");
                server.Prefixes.Add("http://localhost:" + port + "/");
                server.Start(); // начинаем прослушивать входящие подключения
                Console.WriteLine("Сервер начал работу!");

                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                while (isAlive)
                {
                    var inputThread = new Thread(() =>
                    {
                        while (isAlive)
                        {
                            if (Console.ReadLine().ToLower() == "stop")
                            {
                                isAlive = false;
                                cts.Cancel();
                            }
                        }
                    });
                    inputThread.Start();
                    // получаем контекст
                    var context = await server.GetContextAsync();

                    var response = context.Response;
                    // отправляемый в ответ код html возвращает
                    string path = "Google/index.html";
                    string responseText = "";
                    using (StreamReader reader = new StreamReader(path))
                    {
                        string html = await reader.ReadToEndAsync();
                        responseText = html;
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                    // получаем поток ответа и пишем в него ответ
                    response.ContentLength64 = buffer.Length;
                    using Stream output = response.OutputStream;
                    // отправляем данные
                    await output.WriteAsync(buffer);
                    await output.FlushAsync();

                    Console.WriteLine("Запрос обработан");
                }

                server.Stop();
                Console.WriteLine("Сервер закончил работу!");
            }
        }
        else
        {
            throw new ArgumentException("json файл не найден!");
        }
    }
}



