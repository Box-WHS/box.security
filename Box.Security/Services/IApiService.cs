using System;
using System.Threading.Tasks;
using Box.Security.Data.Types;

namespace Box.Security.Services
{
    public interface IApiService
    {
        Task<User> GetUserAsync(Guid userId);
        User GetUser(Guid userId);
        Task AddUserAsync(Guid userId);
        void AddUser(Guid userId);

    }
}