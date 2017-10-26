using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace Box.Security.IdentityServer
{
    public class Config
    {
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("box-api", "Box API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "box-api" }
                },
                new Client
                {
                    ClientId = "box-mvc",
                    ClientName = "Box MVC-Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret("box123".Sha256())
                    },

                    RedirectUris = { "http://localhost:4711/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:4711/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "box-api"

                    },
                    AllowOfflineAccess = true
                }
            };
        }

        internal static IEnumerable<IdentityResource> GetIdentityRessources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResource()
                {
                    Name = "test",
                    DisplayName = "Test User",
                    Description = "Test user for testing",
                    Enabled = true,
                    Required = true,
                    ShowInDiscoveryDocument = true,
                    UserClaims = new List<string>()
                    {
                        "LOGIN"
                    }
                }
            };
        }
    }
}
