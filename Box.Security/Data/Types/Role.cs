using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Box.Security.Data.Types
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}