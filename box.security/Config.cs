using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace box.security
{
    public class Config
    {
        /// <summary>
        /// Gets registered API-resources
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("box-api", "Box API")
            };
        }
        /// <summary>
        /// Gets all registered Clients
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",                                    // Something like username
                    AllowedGrantTypes = GrantTypes.ClientCredentials,       // Auth Mode (use username and password instead of interactive)
                    ClientSecrets =                                         // Client secret
                    {
                        new Secret("secret".Sha256())                       // secret is 'secret' as SHA-256
                    },
                    AllowedScopes =                                         // Allowed Contexts / scopes for this user
                    {
                        "box-api"
                    }
                }
            };
        }
    }
}
