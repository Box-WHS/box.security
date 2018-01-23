using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

namespace Box.Security.Services
{
    public class EmailService : IEmailService
    {
        public string SmtpServer { get; }
        private NetworkCredential SmtpCredentials { get; }
        public string BoxMailAddress { get; }
        public int SmtpPort { get; }

        public EmailService(IConfiguration configuration)
        {
            SmtpServer = configuration["Email:Smtp"];
            SmtpCredentials = new NetworkCredential(configuration["Email:Username"], configuration["Email:Password"]);
            BoxMailAddress = configuration["Email:EmailAddress"];
            SmtpPort = int.Parse(configuration["Email:SmtpPort"]);
        }
        public void SendMail(string content, string destination)
        {
            throw new NotImplementedException();
        }

        public async Task SendMailAsync(string content, string destination)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Box Authentifizierung", BoxMailAddress));
            mimeMessage.To.Add(new MailboxAddress("David van Elk", destination));
            mimeMessage.Subject = "TEST";
            mimeMessage.Body = new TextPart("plain")
            {
                Text = "Hallo Welt!"
            };
            using (var client = new SmtpClient())
            {
                client.Connect(SmtpServer, SmtpPort, true);
                client.Authenticate("box-whs", "BoxWhs2018!");
                await client.SendAsync(mimeMessage);
                Console.WriteLine("Email sent");
                await client.DisconnectAsync(true);
            }
        }
    }
}
