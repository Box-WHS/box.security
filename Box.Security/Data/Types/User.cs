using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Box.Security.Services.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace Box.Security.Data.Types
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Enabled { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        public VerificationData VerificationData { get; set; }
    }
}
