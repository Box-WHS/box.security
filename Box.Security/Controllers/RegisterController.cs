using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data;
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
        [HttpPost("")]
        public async Task<IActionResult> RegisterUser([FromHeader] string username, [FromHeader] string password,
            [FromHeader] string firstName, [FromHeader] string lastName, [FromHeader] string email)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Bad password.");
            }
            if (await DataContext.Users.Where(user => user.UserName.ToLower().Equals(username.ToLower()) || user.Email.ToLower().Equals(email.ToLower())).AnyAsync()
            ) // Check if user already exists
            {
                return BadRequest("The username or E-Mail address are already used by another account.");
            }
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(
                    "The username, password, firstName, lastName or email parameters contain null-values or whitespaces");
            }
            password = password.Sha256();                                                                               // Only use SHA-256 for Password!
            DataContext.Users.Add(new User                                                                              // Add the user to DB
            {
                UserName = username,
                PasswordHash = password,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            });
            await DataContext.SaveChangesAsync();                                                                       // Commit changes to DB

            return NoContent();
        }
    }
}
