using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configurations
{
    public class IdentityServerConfiguration : IIdentityServerConfiguration
    {
        private readonly MobileClientConfiguration _mobileClientConfiguration;
        
        public IdentityServerConfiguration(MobileClientConfiguration mobileClientConfiguration)
        {
            _mobileClientConfiguration = mobileClientConfiguration;
        }

        public IEnumerable<Client> GetClients() =>
            new Client[]
            {
                   new()
                   {
                        ClientId = "Сatalog-Client",
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
                        RedirectUris = new List<string>{ "http://localhost:4200", "https://oauth.pstmn.io/v1/callback" },
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
                       AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                       AllowOfflineAccess = true,
                       RefreshTokenUsage = TokenUsage.ReUse,
                       RefreshTokenExpiration = TokenExpiration.Absolute,
                       ClientSecrets =
                       {
                           new Secret { Value = "ClientSecret1".Sha256()}
                       },
                       AllowedScopes = { 
                           "CatalogAPI.read", 
                           "CatalogAPI.write", 
                           IdentityServerConstants.StandardScopes.Profile, 
                           IdentityServerConstants.StandardScopes.OpenId, 
                       }
                   },
            };

        public IEnumerable<ApiScope> GetApiScopes() =>
           new ApiScope[]
           {
               new("CatalogAPI.read"),
               new("CatalogAPI.write")
           };

        public IEnumerable<ApiResource> GetApiResources() =>
          new ApiResource[]
          {
               new("CatalogAPI")
               {
                   Scopes = new string[] { "CatalogAPI.read" , "CatalogAPI.write" },
                   ApiSecrets = new Secret[] {new("ScopeSecret".Sha256())},
                   UserClaims = new string[] { "role"}
               }
          };

        public IEnumerable<IdentityResource> GetIdentityResources() =>
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
