using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Box.Security.Data;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;

namespace Box.Security.Services
{
    public class ProfileService : IProfileService
    {
        private UserDataContext DataContext { get; }

        public ProfileService(UserDataContext context)
        {
            DataContext = context;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var dbUser = await DataContext.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Subject, dbUser.Id));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            if (context.Subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            return Task.FromResult(0);
        }
    }
}
