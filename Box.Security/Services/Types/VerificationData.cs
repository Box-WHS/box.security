using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data.Types;

namespace Box.Security.Services.Types
{
    public class VerificationData : IVerificationData
    {
        public User User { get; set; }
        public Guid Id { get; set; }
        public string Dirt { get; set; }
    }
}
