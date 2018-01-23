using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Box.Security.Data;
using Box.Security.Data.Types;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
// ReSharper disable ClassNeverInstantiated.Global

namespace Box.Security.Validation
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private UserDataContext DataContext { get; }

        public ResourceOwnerPasswordValidator(UserDataContext context)
        {
            DataContext = context;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var dbUser = await DataContext.Users
                .Where(user => user.UserName.ToLower() == context.UserName.ToLower() || user.Email.ToLower() == context.UserName.ToLower())
                .FirstOrDefaultAsync();

            if (dbUser == null)
            {
                context.Result.ErrorDescription = "The user does not exist.";
                context.Result.IsError = true;
                context.Result.Error = "User does not exist.";
                return;
            }
            if (!dbUser.Enabled)
            {
                context.Result.ErrorDescription = $"The user { dbUser.UserName } is not enabled.";
                context.Result.IsError = true;
                context.Result.Error = "User not enabled.";
            }
            else if (dbUser.AccessFailedCount == 5)
            {
                dbUser.Enabled = false;
                context.Result.IsError = true;
                context.Result.ErrorDescription = $"The user { dbUser.UserName } has been disabled due to 5 invalid login attempts.";
                context.Result.Error = "User disabled";
            }
            else if (dbUser.PasswordHash == context.Password.Sha256())
            {
                dbUser.AccessFailedCount = 0;
                context.Result = new GrantValidationResult(
                    subject: dbUser.Id,
                    authenticationMethod: "password",
                    claims: GetUserClaimsAsync(dbUser));
            }
            else
            {
                dbUser.AccessFailedCount++;
            }
            DataContext.Update(dbUser);
            await DataContext.SaveChangesAsync();
        }

        private IEnumerable<Claim> GetUserClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", user.Id),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Email, user.Email)
            };
            return claims;
        }
    }
}
