using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configurations
{
    public class IdentityServerConfiguration
    {
        public IdentityServerConfiguration()
        {
          
        }
        
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                   new()
                   {
                        ClientId = "СatalogClient",
                        ClientName = "Сatalog Credentials Client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets =
                        {
                            new Secret { Value = "ClientSecret1".Sha256()}
                        },
                        AllowedScopes = { "CatalogAPI.read", "CatalogAPI.write" }
                   },
                   new()
                   {
                        ClientId = "Angular-Client",
                        ClientName = "angular-client",
                        AllowedGrantTypes = GrantTypes.Code,
                        RedirectUris = new List<string>{ "http://localhost:4200" },
                        RequirePkce = true,
                        AllowAccessTokensViaBrowser = true,
                        AllowedScopes = {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                        },
                        AllowedCorsOrigins = { "http://localhost:4200" },
                        RequireClientSecret = false,
                        PostLogoutRedirectUris = new List<string> { "http://localhost:4200" },
                        RequireConsent = false,
                        AccessTokenLifetime = 600,
                    },
                   new()
                   {
                       ClientId = "Maui-Client",
                       ClientName = "maui-client",
                       AllowedGrantTypes = GrantTypes.Code,
                      
                       RedirectUris = new List<string>{ "https://oauth.pstmn.io/v1/callback" },
                       RequireConsent = false,
                       RequirePkce = true,
                       RequireClientSecret = false,
                       PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                       //AllowedCorsOrigins = { "http://localhost:4200" },
                       AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                       },
                       AllowAccessTokensViaBrowser = true
                   },
                   new()
                   {
                       ClientId = "Maui-Client-Credentials",
                       ClientName = "maui-client Credentials",
                       AllowedGrantTypes = GrantTypes.ClientCredentials,
                       ClientSecrets =
                       {
                           new Secret { Value = "ClientSecret1".Sha256()}
                       },
                       AllowedScopes = { "CatalogAPI.read", "CatalogAPI.write" }
                   },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new("CatalogAPI.read"),
               new("CatalogAPI.write")
           };

        public static IEnumerable<ApiResource> ApiResources =>
          new ApiResource[]
          {
               new("CatalogAPI")
               {
                   Scopes = new string[] { "CatalogAPI.read" , "CatalogAPI.write" },
                   ApiSecrets = new Secret[] {new("ScopeSecret".Sha256())},
                   UserClaims = new string[] { "role"}
               }
          };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Email(),
              new()
              {
                  Name = "role",
                  UserClaims = new string[] {"role"}
              }

          };
    }
}
