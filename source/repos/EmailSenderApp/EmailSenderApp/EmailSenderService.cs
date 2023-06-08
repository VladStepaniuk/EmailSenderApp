using System.Net;
using System.Net.Mail;

namespace EmailSenderApp
{
    public class EmailSenderService
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail()
        {
            string smtpServer = _configuration.GetValue<string>("Email:SmtpServer");
            int smtpPort = _configuration.GetValue<int>("Email:SmtpPort");
            string email = _configuration.GetValue<string>("Email:Email");
            string password = _configuration.GetValue<string>("Email:Password");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(email);
            message.To.Add("receiver@example.com");
            message.Subject = "Hello World!";
            message.Body = "Hello World!";

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(email, password);
            smtpClient.EnableSsl = true;


            smtpClient.Send(message);
        }
    }
}
