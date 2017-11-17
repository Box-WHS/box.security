using Box.Security.Data.Types.Interfaces;

namespace Box.Security.Data.Types
{
    public class AuthorizationRole : IAuthorizationRole
    {
        public int Id { get; set; }
        public Authorization Authorization { get; set; }
        public Role Role { get; set; }
    }
}