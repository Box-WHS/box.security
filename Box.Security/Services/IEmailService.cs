using System.Threading.Tasks;

namespace Box.Security.Services
{
    public interface IEmailService
    {
        void SendMail(string content, string subject, string destination);
        Task SendMailAsync(string content, string subject, string destination);
    }
}
