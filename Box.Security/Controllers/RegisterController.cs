using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data;
using Box.Security.Data.TransferData;
using Box.Security.Data.Types;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Box.Security.Controllers
{
    [Route("register")]
    public class RegisterController : Controller
    {
        private UserDataContext DataContext { get; }

        public RegisterController(UserDataContext dataContext)
        {
            DataContext = dataContext;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns>HTTP-Response Code</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserData user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await DataContext.Users
                .Where(usr =>
                    usr.UserName.ToLower().Equals(user.UserName.ToLower()) ||
                    usr.Email.ToLower().Equals(user.Email.ToLower()))
                .AnyAsync())
            {
                return BadRequest("Another user with this userName or email already exists.");
            }

            var userDto = (await DataContext.Users.AddAsync(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.Password.Sha256(),
                UserName = user.UserName
            })).Entity;

            await DataContext.AuthorizationRoles
                .Where(authRole => authRole.Role.SysName.Equals("role:normal"))
                .Include(authRole => authRole.Authorization)
                .Include(authRole => authRole.Role)
                .ForEachAsync(authRole =>
                {
                    DataContext.AuthorizationUsers.Add(new AuthorizationUser
                    {
                        Authorization = authRole.Authorization,
                        User = userDto
                    });
                });
            
            await DataContext.SaveChangesAsync();

            return Ok(userDto);
        }
    }
}
