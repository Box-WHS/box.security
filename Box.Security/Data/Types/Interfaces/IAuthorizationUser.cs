using System;

namespace Box.Security.Data.Types.Interfaces
{
    public interface IAuthorizationUser
    {
        int Id { get; set; }
        User User { get; set; }
        Authorization Authorization { get; set; }
    }
}