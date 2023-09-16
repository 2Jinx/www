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
            
            const string staticFilesPath = "static";
            const string HtmlPath = $"{staticFilesPath}/index.html";
            bool isAlive = true;

            try
            { 
                HttpListener server = new HttpListener();
                ServerConfiguration configuration = new ServerConfiguration();
                configuration.Set(server);

                Console.WriteLine("The server started working!");

                server.Start();

                var context = await server.GetContextAsync();
                var response = context.Response;

                if (!Directory.Exists(staticFilesPath))
                {
                    Directory.CreateDirectory(staticFilesPath);
                }

                string responseText = "";
                if (!File.Exists(HtmlPath))
                {
                    responseText = $"Error 404. File {HtmlPath} not found!";
                }
                else
                {
                    using (StreamReader reader = new StreamReader(HtmlPath))
                    {
                        string html = await reader.ReadToEndAsync();
                        responseText = html;
                    }
                }

                byte[] buffer = Encoding.UTF8.GetBytes(responseText);

                response.ContentLength64 = buffer.Length;
                using Stream output = response.OutputStream;

                await output.WriteAsync(buffer);
                await output.FlushAsync();
                Console.WriteLine("Request processed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("The server has finished working!");
            }
        }
    }
}
