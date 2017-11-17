using Box.Security.Data.Types.Interfaces;

namespace Box.Security.Data.Types
{
    public class AuthorizationUser : IAuthorizationUser
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Authorization Authorization { get; set; }
    }
}