using System.Collections.Generic;
using Box.Security.Data.Types.Interfaces;

namespace Box.Security.Data.Types
{
    public class Role : IRole
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<AuthorizationRole> Authorizations { get; set; }
        public string SysName { get; set; }
    }
}