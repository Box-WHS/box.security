using Microsoft.AspNetCore.Identity;

namespace Box.Security.Data.Types.Interfaces
{
    public interface IAuthorizationRole
    {
        int Id { get; set; }
        Authorization Authorization { get; set; }
        Role Role { get; set; }
    }
}