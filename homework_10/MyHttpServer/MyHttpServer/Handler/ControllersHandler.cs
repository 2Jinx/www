using System.Net;
using System.Web;
using System.Reflection;
using MyHttpServer.Configuration;
using MyHttpServer.Attributes;
using System.Text;
using MyHttpServer.Model;
using System.Text.Json;

namespace MyHttpServer.Handler
{
    public class ControllersHandler: IHandler
    {
        private ServerConfiguration _configuration;

        public ControllersHandler(ServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void Handle(HttpListenerContext context)
        {
            try
            {
                var strParams = context?.Request.Url!
                    .Segments
                    .Skip(1)
                    .Select(s => s.Replace("/", ""))
                    .ToArray();

                if (strParams!.Length >= 2)
                {
                    string input = await new StreamReader(context.Request!.InputStream).ReadToEndAsync();
                    
                    var queryParams = HttpUtility.ParseQueryString(input);
                    List<object> parameterValues = new List<object>();

                    foreach (var key in queryParams.AllKeys)
                    {
                        parameterValues.Add(queryParams[key]);
                    }

                    string controllerName = strParams[0], methodName = strParams[1];
                    var assembly = Assembly.GetExecutingAssembly();

                    var controller = assembly.GetTypes()
                        .Where(t => Attribute.IsDefined(t, typeof(ControllerAttribute)))
                        .FirstOrDefault(c => ((ControllerAttribute)Attribute.GetCustomAttribute(c, typeof(ControllerAttribute))!)
                        .Type.Equals(controllerName, StringComparison.OrdinalIgnoreCase));

                    var method = controller?.GetMethods()
                       .Where(x => x.GetCustomAttributes(true)
                       .Any(attr => attr.GetType().Name.Equals($"{context.Request.HttpMethod}Attribute", StringComparison.OrdinalIgnoreCase)))
                       .FirstOrDefault(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));

                    if (controller.ToString().Contains("Authentication"))
                        parameterValues.Add(_configuration);

                    if (parameterValues.Count == 0 && strParams.Length > 2 && controller.ToString().Contains("Account"))
                    {
                        for (int i = 2; i < strParams.Length; i++)
                            parameterValues.Add(strParams[i]);
                    }

                    var result = method?.Invoke(Activator.CreateInstance(controller), parameterValues.ToArray());

                    if (result != null)
                    {
                        if (result is Account || result is Account[])
                        {
                            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
                            SendHtml(context, json);
                        }
                    }

                    context.Response.Close();
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Controller handler: " + ex.Message);
                Console.ResetColor();
            }
        }

        private async void SendHtml(HttpListenerContext context, string html)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(html);
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = buffer.Length;

            using Stream output = context.Response.OutputStream;
            await output.WriteAsync(buffer);
            await output.FlushAsync();
        }
    }
}

