using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data.Types;
using Box.Security.Services.Types;

namespace Box.Security.Services
{
    public interface IAccountVerificationService
    {
        Task SendVerificationMailAsync(User user);
        void SendVerificationMail(User user);
        Task<bool> VerifyUserAccountAsync(string userId, Guid id, string dirt);
        bool VerifyUserAccount(string userId, Guid id, string dirt);
    }
}
