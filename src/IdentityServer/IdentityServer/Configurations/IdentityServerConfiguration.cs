using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configurations
{
    public class IdentityServerConfiguration
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                   new Client
                   {
                        ClientId = "СatalogClient",
                        ClientName = "Сatalog Credentials Client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets =
                        {
                            new Secret("ClientSecret1")
                        },
                        AllowedScopes = { "CatalogAPI.read", "CatalogAPI.write" }
                   },

                   // interactive client using code flow + pkce
                   new Client
                   {
                     ClientId = "interactive",
                     ClientSecrets = {new Secret("ClientSecret1")},

                     AllowedGrantTypes = GrantTypes.Code,

                     RedirectUris = {"https://localhost:7059/signin-oidc"},
                     FrontChannelLogoutUri = "https://localhost:7059/signout-oidc",
                     PostLogoutRedirectUris = {"https://localhost:7059/signout-callback-oidc"},

                     AllowOfflineAccess = true,
                     AllowedScopes = {"openid", "profile", "CatalogAPI.read"},
                     RequirePkce = true,
                     RequireConsent = true,
                     AllowPlainTextPkce = false
                   },
                    new Client
                    {
                        ClientId = "Angular-Client",
                        ClientName = "angular-client",
                        AllowedGrantTypes = GrantTypes.Code,
                        RedirectUris = new List<string>{ "http://localhost:4200/signin-callback", "http://localhost:4200/assets/silent-callback.html" },
                        RequirePkce = true,
                        AllowAccessTokensViaBrowser = true,
                        AllowedScopes = {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                        },
                        AllowedCorsOrigins = { "http://localhost:4200" },
                        RequireClientSecret = false,
                        PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                        RequireConsent = false,
                        AccessTokenLifetime = 600,
                    }         
            };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("CatalogAPI.read"),
               new ApiScope("CatalogAPI.write")
           };

        public static IEnumerable<ApiResource> ApiResources =>
          new ApiResource[]
          {
               new ApiResource("CatalogAPI")
               {
                   Scopes = new string[] { "CatalogAPI.read" , "CatalogAPI.write" },
                   ApiSecrets = new Secret[] {new Secret("ScopeSecret".Sha256())},
                   UserClaims = new string[] { "role"}
               }
          };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Email(),
              new IdentityResource
              {
                  Name = "role",
                  UserClaims = new string[] {"role"}
              }

          };
    }
}
