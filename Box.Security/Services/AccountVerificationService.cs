using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box.Security.Data;
using Box.Security.Data.Types;
using Box.Security.Services.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Box.Security.Services
{
    public class AccountVerificationService : IAccountVerificationService
    {
        private UserDataContext DataContext { get; }
        private IEmailService EmailService { get; }
        private IConfiguration Configuration { get; }
        private IApiService ApiService { get; }

        private readonly string ZUUL_PROXY;
        public AccountVerificationService(UserDataContext dataContext,
            IEmailService emailService,
            IConfiguration configuration,
            IApiService apiService)
        {
            DataContext = dataContext;
            EmailService = emailService;
            Configuration = configuration;
            ApiService = apiService;

            ZUUL_PROXY = Configuration["ZuulProxy"];
        }
        public async Task SendVerificationMailAsync(User user)
        {
            var verificationData = new VerificationData
            {
                User = user,
                Dirt = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id + Guid.NewGuid())) //Do some random stuff...
            };
            await DataContext.Verifications.AddAsync(verificationData);
            await DataContext.SaveChangesAsync();

            var url = $"{ZUUL_PROXY}/auth/verify/{verificationData.Id}?u={verificationData.User.Id}&d={verificationData.Dirt}";
            var mailText = $"Hi {user.FirstName},\n\nWelcome to Box!\n\nPlease verify you account by clicking the link:\n" +
                       $"{url}\n" +
                       $"Have fun!\nYour Box Team :)";
            await EmailService.SendMailAsync(mailText, "Verify your new Box Account!", verificationData.User.Email);
        }

        public void SendVerificationMail(User user)
        {
            SendVerificationMailAsync(user).GetAwaiter().GetResult();
        }

        public async Task<bool> VerifyUserAccountAsync(string userId, Guid id, string dirt)
        {
            VerificationData db;
            if ((db = await DataContext.Verifications
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(v => v.Id == id)) != null)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                if (db.User.Enabled = db.User.Id.Equals(userId) &&
                    db.Dirt.Equals(dirt) &&
                    db.Id == id)
                {
                    DataContext.Users.Update(db.User);
                    var __saveDbTask = DataContext.SaveChangesAsync();
                    var __addUserTask = ApiService.AddUserAsync(Guid.Parse(userId));   //POST User to API

                    await __addUserTask;
                    await __saveDbTask;
                    return true;
                }
            }
            return false;
        }

        public bool VerifyUserAccount(string userId, Guid id, string dirt)
        {
            return VerifyUserAccountAsync(userId, id, dirt).GetAwaiter().GetResult();
        }
    }
}
