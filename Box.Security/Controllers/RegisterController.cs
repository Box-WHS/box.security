using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data;
using Box.Security.Data.Types;
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
        /// <param name="newUser">JSON-Object of the new User</param>
        /// <returns>HTTP-Response Code</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] User newUser)
        {
            //Validate new User
            if (await DataContext.Users.Where(user => user.UserName == newUser.UserName || user.Email == newUser.Email)
                .AnyAsync())
            {
                return BadRequest("Another user with the same Username or E-Mail-address already exists");
            }
            else if (string.IsNullOrWhiteSpace(newUser.PasswordHash) || string.IsNullOrWhiteSpace(newUser.FirstName) ||
                     string.IsNullOrWhiteSpace(newUser.LastName))
            {

                return BadRequest(
                    "The given user-parameter does not contain a valid Password, FirstName or LastName.");
            }
            else
            {
                await DataContext.Users.AddAsync(newUser);
                await DataContext.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
