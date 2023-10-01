using System.Net;
using MyHttpServer.Controllers;
using System.Web;
using System.Reflection;
using MyHttpServer.Configuration;

namespace MyHttpServer.Handler
{
    public class ControllersHandler: IHandler
    {
        private ServerConfiguration _configuration;
        private readonly string _sitePreset;
        public ControllersHandler(string sitePreset, ServerConfiguration configuration)
        {
            _configuration = configuration;
            _sitePreset = sitePreset;
        }

        public async void Handle(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string absolutePath = request.Url!.AbsolutePath;
            if(absolutePath == "/authentication/send-email")
            {
                string input = await new StreamReader(request.InputStream).ReadToEndAsync(),
                    email = HttpUtility.UrlDecode(input.Split('&')[0].Split('=')[1]),
                    password = HttpUtility.UrlDecode(input.Split('&')[1].Split('=')[1]);

                string[] mailParams = { email, password };

                Type authenticationControllerType = typeof(AuthenticationController);
                ConstructorInfo constructorInfo = authenticationControllerType.GetConstructor(new Type[] { _configuration.GetType() });
                object[] constructorParameters = new object[] { _configuration };
                object authenticationControllerInstance = constructorInfo.Invoke(constructorParameters);

                MethodInfo sendEmailInfo = authenticationControllerType.GetMethod("SendMail", BindingFlags.Instance | BindingFlags.NonPublic);
                sendEmailInfo.Invoke(authenticationControllerInstance, mailParams);

                response.Redirect("/");
                response.Close();
            }
            else
            {
                new StaticFilesHandler(_sitePreset, _configuration).Handle(context);
            }
        }
    }
}

