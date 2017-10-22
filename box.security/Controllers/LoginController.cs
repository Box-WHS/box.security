using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IdentityModel.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace box.security.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        /// <summary>
        /// Configuration-Object, gets config from appsettings.json
        /// </summary>
        IConfiguration Config { get; }
        /// <summary>
        /// URL of IdentityServer
        /// </summary>
        string IdentityServer { get; }

        /// <summary>
        /// DI Injector and Constructor
        /// </summary>
        public LoginController(IConfiguration config)
        {
            Config = config;                                                                                                //Set the Config-object to the injected IConfiguration from DI
            IdentityServer = Config.GetSection("Configuration").GetValue<string>("IdentityEndpoint");                       //Gets the URL of the server from appsettings.json

        }
        /// <summary>
        /// Creates a token on valid login, parameters can be read from header
        /// </summary>
        /// <returns>200 on success (token in body), 400 on clients fail and 500 on server fail</returns>
        [HttpGet]
        public async Task<IActionResult> Login([FromHeader] string username, [FromHeader] string password)
        {
            var discoveryEndpoint = await DiscoveryClient.GetAsync(IdentityServer);                                         //Discover all endpoints into a discovery-object
            var tokenEndpoint = new TokenClient(discoveryEndpoint.TokenEndpoint, username, password);                       //Create a new token-endpoint
            var token = await tokenEndpoint.RequestClientCredentialsAsync("box-api");                                       //Request the token from the server

            if(token.IsError)                                                                                               //On Error: return a 400 with some information
            {
                return BadRequest(token.ErrorDescription);
            }

            return Ok(token.Json);                                                                                          //Everything is good! return 200 with JWT!
        }
    }
}
