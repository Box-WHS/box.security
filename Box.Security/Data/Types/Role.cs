using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Box.Security.Data.Types
{
    public class Role : IdentityRole
    {
        public IEnumerable<Policy> Policies { get; set; }
    }
}
