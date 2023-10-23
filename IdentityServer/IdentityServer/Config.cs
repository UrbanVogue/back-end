
using IdentityServer4.Models;

namespace IdentityServer
{
    public class Config
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
