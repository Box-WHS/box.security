﻿using System;
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
    /// <summary>
    /// Overriden ProfileService, IdentityServer.ProfileService always returns IsActive() = false. And that is shit.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private UserDataContext DataContext { get; }

        /// <summary>
        /// Constructor of ProfileService, use DI to inject the UserDataContext
        /// </summary>
        /// <param name="context"></param>
        public ProfileService(UserDataContext context)
        {
            DataContext = context;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var dbUser = await DataContext.Users
                .Where(user => user.Id == userId)
                .FirstOrDefaultAsync();
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Subject, dbUser.Id));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, dbUser.Email));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.GivenName, dbUser.FirstName));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.FamilyName, dbUser.LastName));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, "Marcel stinkt"));
            
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Scope, "deinemama"));
            foreach (var subjectClaim in context.Subject.Claims)
            {
                Console.WriteLine(subjectClaim.Value);
            }
        }

        /// <summary>
        /// Override the IsActiveAsync-Method to prevent "User with id { xxxx } is not enabled
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
