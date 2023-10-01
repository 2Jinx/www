using MyHttpServer.Services;
using MyHttpServer.Configuration;

namespace MyHttpServer.Controllers
{
    [Controller("/authentication")]
    public class AuthenticationController
    {
        private ServerConfiguration _configuration;

        public AuthenticationController(ServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Controller("/send-email")]
        private void SendMail(string email, string password)
        {
            EmailSender sender = new EmailSender(_configuration);
            sender.SendEmail(email, password);
        }
    }
}

