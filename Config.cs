using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankOfDotNet.IdentitySvr
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Michael",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "Bob",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("bankOfDotNetIdentityServer4Api", "Customer Api for BankOfDotNet")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //Client-credential based grant type
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankOfDotNetIdentityServer4Api" }
                },

                //resources owner password based grant type
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankOfDotNetIdentityServer4Api" }
                },

                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    RedirectUris = {"http://localhost:5003/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5003/signout-callback-oidc"},

                    AllowedScopes = new List<String>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },

                //Swagger client
                new Client
                {
                    ClientId = "swaggerapiui",
                    ClientName = "Swagger API UI",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = {"http://localhost:59337/swagger/oauth2-redirect.html"},
                    PostLogoutRedirectUris = {"http://localhost:59337/swagger"},

                    AllowedScopes = { "bankOfDotNetIdentityServer4Api" },
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}
