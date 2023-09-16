using System.Net;
using System.Text;
using System.Text.Json;
using Configuration.MyHttpServer;

namespace MyHttpServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string configFilePath = "appsetting.json";
            try
            {
                if (!File.Exists(configFilePath))
                {
                    Console.WriteLine("json файл не найден!");
                    throw new Exception();
                }
                AppSettings config;
                using (FileStream file = File.OpenRead(configFilePath))
                {
                    config = JsonSerializer.Deserialize<AppSettings>(file);
                }

                HttpListener server = new HttpListener();
                // установка адресов прослушки
                server.Prefixes.Add($"{config.Address}:{config.Port}/connection/");
                server.Prefixes.Add($"http://localhost:{config.Port}/");
                server.Start(); // начинаем прослушивать входящие подключения
                Console.WriteLine("Сервер начал работу!");

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

                server.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Сервер закончил работу!");
            }
        }
    }
}
