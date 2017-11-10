using System;
using System.Collections.Generic;
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
        UserDataContext DataContext { get; }

        public RegisterController(UserDataContext dataContext)
        {
            DataContext = dataContext;
        }

        /// <summary>
        /// Registers a new user, expects password to be a SHA256-Hash.
        /// </summary>
        /// <returns>HTTP-Response Code</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserData user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Password = user.Password.Sha256();
            if (await DataContext.Users
                .Where(usr =>
                    usr.UserName.ToLower().Equals(user.UserName.ToLower()) ||
                    usr.Email.ToLower().Equals(user.Email.ToLower()))
                .AnyAsync())
            {
                return BadRequest("Another user with this userName or email already exists.");
            }
            await DataContext.Users.AddAsync(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.Password,
                UserName = user.UserName,

            });
            await DataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
