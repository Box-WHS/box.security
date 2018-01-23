using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data.Types;

namespace Box.Security.Services.Types
{
    public interface IVerificationData
    {
        User User { get; set; }
        Guid Id { get; set; }
        string Dirt { get; set; }
    }
}
