namespace Services.MyHttpServer
{
    public interface IEmailSenderService
    {
        public void SendEmail(string email, string password);
    }
}

