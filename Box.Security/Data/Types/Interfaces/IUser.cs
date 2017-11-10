using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Box.Security.Data.Types.Interfaces
{
    interface IUser
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        bool Enabled { get; set; }
    }
}
