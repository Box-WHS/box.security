using System.Collections.Generic;

namespace Box.Security.Data.Types.Interfaces
{
    public interface IRole
    {
        int RoleId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        IEnumerable<AuthorizationRole> Authorizations { get; set; }
        string SysName { get; set; }
    }
}