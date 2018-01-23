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
        public void SendMail(string content, string subject, string destination)
        {
            SendMailAsync(content, subject, destination).GetAwaiter().GetResult();
        }

        public async Task SendMailAsync(string content, string subject, string destination)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Box WHS TestMail", BoxMailAddress));
            mimeMessage.To.Add(new MailboxAddress("David van Elk", "david.vanelk96@gmail.com"));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = content
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(SmtpServer, SmtpPort, true);
                await client.AuthenticateAsync(SmtpCredentials.UserName, SmtpCredentials.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
