using System;
using System.Net;
using System.Text.Json;
using Configuration.MyHttpServer;

namespace MyHttpServer
{
    public class ServerConfiguration
    {
        const string configFilePath = "appsettings.json";

        /// <summary>
        /// Server configuration settings
        /// </summary>
        /// <param name="httplistener"></param>
        public void Set(HttpListener httplistener)
        {
            try
            {
                if (!File.Exists(configFilePath))
                {
                    Console.WriteLine("json file not found!");
                    throw new Exception();
                }
                AppSettings config;
                using (FileStream file = File.OpenRead(configFilePath))
                {
                    config = JsonSerializer.Deserialize<AppSettings>(file);
                }

                httplistener.Prefixes.Add($"{config.Address}:{config.Port}/connection/");
                httplistener.Prefixes.Add($"http://localhost:{config.Port}/");
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Configuration: {ex.Message}");
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Configuration: All configurations set!");
            }
        }
    }
}

