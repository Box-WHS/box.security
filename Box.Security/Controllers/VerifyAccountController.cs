using Box.Security.Data;
using Box.Security.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Box.Security.Controllers
{
    [Route("verify")]
    public class VerifyAccountController : Controller
    {
        private UserDataContext DataContext { get; }
        private IAccountVerificationService VerificationService { get; }

        public VerifyAccountController(UserDataContext dataContext,
            IAccountVerificationService verificationService)
        {
            DataContext = dataContext;
            VerificationService = verificationService;
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> VerifyUserAsync([FromRoute]Guid id, string u, string d)
        {
            if (await VerificationService.VerifyUserAccountAsync(u, id, d))
                return Ok("User verified");
            return BadRequest("User could not be verified.");
        }

    }
}
