using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Box.Security.Data;
using Box.Security.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

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
                return Ok();
            else
                return BadRequest("User could not be verified.");
        }

    }
}
