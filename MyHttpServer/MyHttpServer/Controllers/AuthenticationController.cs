using MyHttpServer.Services;
using MyHttpServer.Configuration;
using MyHttpServer.Model;
using MyHttpServer.Attributes;

namespace MyHttpServer.Controllers
{
    [Controller("Authentication")]
    public class AuthenticationController
    {
        [Post("SendEmail")]
        public void SendEmail(string email, string password, ServerConfiguration configuration)
        {
            EmailSender sender = new EmailSender(configuration);
            sender.SendEmail(email, password);
        }

        [Get("GetEmailList")]
        public string GetEmailList()
        {
            return "<html><head><body><p>метод GetEmailList</p></body></head></html>";
        }

        [Get("GetAccountsList")]
        public Account[] GetAccountsList()
        {
            var accounts = new[]
            {
                new Account{Email = "sdngdk", Password = "dngdkg"},
                new Account{Email = "dnkg", Password = "dbghdl"}
            };

            return accounts;
        }
    }
}

