﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Box.Security
{
    public static class Config
    {
        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("box-security-api", "Box Security API")
            };
        }

        public static List<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "box",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "box-security-api" }
                },
                new Client()
                {
                    ClientId = "box-ng-client",
                    ClientName = "Angular Box Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:4200/" },
                    PostLogoutRedirectUris = { "http://localhost:4200/" },
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "box-security-api"
                    }

                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "david.elk",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}