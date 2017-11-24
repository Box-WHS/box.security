using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data;
using Box.Security.Data.TransferData;
using Box.Security.Data.Types;
using Box.Security.Services;
using Box.Security.Services.Types;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Box.Security.Controllers
{
    [Route("register")]
    public class RegisterController : Controller
    {
        private UserDataContext DataContext { get; }
        private IConfiguration Configuration { get; }
        private ICaptchaService CaptchaService { get; }

        public RegisterController(UserDataContext dataContext, IConfiguration config, ICaptchaService captchaService)
        {
            DataContext = dataContext;
            Configuration = config;
            CaptchaService = captchaService;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <returns>HTTP-Response Code</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserData user)
        {
            CaptchaResponse captchaResponse;
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!(captchaResponse = await CaptchaService.CaptchaSolvedAsync(user.Captcha)).Success)
            {
                return BadRequest("The reCaptcha is not solved! Details: " +
                                  JsonConvert.SerializeObject(captchaResponse.ErrorCodes));
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
