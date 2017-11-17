using System.Collections.Generic;

namespace Box.Security.Data.Types.Interfaces
{
    public interface IAuthorization
    {
        int AuthorizationId { get; set; }
        string Name { get; set; }
        string SysName { get; set; }
        string Description { get; set; }
        IEnumerable<AuthorizationUser> AuthorizationUsers { get; set; }
        IEnumerable<AuthorizationRole> AuthorizationRoles { get; set; }
    }
}