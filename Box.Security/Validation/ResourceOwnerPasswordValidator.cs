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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
                .Where(user => user.UserName == context.UserName)
                .FirstOrDefaultAsync();
            if (!dbUser.Enabled)
            {
                context.Result.ErrorDescription = "The user is not enabled yet.";
                context.Result.IsError = true;
                context.Result.Error = "User not enabled.";
            }
            if (dbUser?.PasswordHash == context.Password.Sha256())
            {
                context.Result = new GrantValidationResult(
                    subject: dbUser.Id,
                    authenticationMethod: "custom",
                    claims: GetUserClaimsAsync(dbUser)); 
            }
        }

        IEnumerable<Claim> GetUserClaimsAsync(User user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("user_id", user.Id));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtClaimTypes.Role, JsonConvert.SerializeObject(user.Role)));
            return claims;
        }
    }
}
