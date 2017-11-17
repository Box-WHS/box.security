using System.Collections.Generic;
using Box.Security.Data.Types.Interfaces;

namespace Box.Security.Data.Types
{
    public class Authorization : IAuthorization
    {
        public int AuthorizationId { get; set; }
        public string Name { get; set; }
        public string SysName { get; set; }
        public string Description { get; set; }
        public IEnumerable<AuthorizationUser> AuthorizationUsers { get; set; }
        public IEnumerable<AuthorizationRole> AuthorizationRoles { get; set; }
    }
}