using IdentityModel;
using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<AspNetCoreIdentityDbContext>(
                options => options.UseSqlServer(connectionString)
            );

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetCoreIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );
            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<AspNetCoreIdentityDbContext>();
            ctx.Database.Migrate();
            EnsureUsers(scope);
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var angella = userMgr.FindByNameAsync("angella").Result;
            if (angella == null)
            {
                angella = new IdentityUser
                {
                    UserName = "angella",
                    Email = "angella.freeman@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(angella, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result =
                    userMgr.AddClaimsAsync(
                        angella,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Angella Freeman"),
                            new Claim(JwtClaimTypes.GivenName, "Angella"),
                            new Claim(JwtClaimTypes.FamilyName, "Freeman"),
                            new Claim(JwtClaimTypes.WebSite, "http://angellafreeman.com"),
                            new Claim("location", "somewhere")
                        }
                    ).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients.ToList())
                {

                    var entity = new IdentityServer4.EntityFramework.Entities.Client
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName,
                        AllowedGrantTypes = client.AllowedGrantTypes.Select(type => new IdentityServer4.EntityFramework.Entities.ClientGrantType
                        {
                            GrantType = type
                        }).ToList(),
                        ClientSecrets = client.ClientSecrets.Select(secret => new IdentityServer4.EntityFramework.Entities.ClientSecret
                        {
                            Value = secret.Value.Sha256()
                        }).ToList(),
                        AllowedScopes = client.AllowedScopes.Select(scope => new IdentityServer4.EntityFramework.Entities.ClientScope
                        {
                            Scope = scope
                        }).ToList(),
                        PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(uri => new IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri
                        {
                            PostLogoutRedirectUri = uri
                        }).ToList(),
                        RedirectUris = client.RedirectUris.Select(uri => new IdentityServer4.EntityFramework.Entities.ClientRedirectUri
                        {
                            RedirectUri = uri
                        }).ToList(),
                        FrontChannelLogoutUri = client.FrontChannelLogoutUri,   
                        AllowOfflineAccess = client.AllowOfflineAccess,
                        RequirePkce = client.RequirePkce,
                        RequireConsent = client.RequireConsent,
                        AllowPlainTextPkce = client.AllowPlainTextPkce
                    };
                    context.Clients.Add(entity);
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    var entity = new IdentityServer4.EntityFramework.Entities.IdentityResource
                    {
                        Name = resource.Name,
                        DisplayName = resource.DisplayName,
                        Description = resource.Description,
                        Emphasize = resource.Emphasize,
                        Enabled = resource.Enabled,
                        Required = resource.Required,
                        ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                        UserClaims = resource.UserClaims.Select(claim => new IdentityServer4.EntityFramework.Entities.IdentityResourceClaim
                        {
                            Type = claim
                        }).ToList()
                    };

                    context.IdentityResources.Add(entity);
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    var entity = new IdentityServer4.EntityFramework.Entities.ApiScope
                    {
                        Name = resource.Name,
                        DisplayName = resource.DisplayName,
                        Description = resource.Description,
                        Required = resource.Required,
                        Emphasize = resource.Emphasize,
                        ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                        UserClaims = resource.UserClaims.Select(claim => new IdentityServer4.EntityFramework.Entities.ApiScopeClaim
                        {
                            Type = claim
                        }).ToList(),
                        Properties = resource.Properties.Select(prop => new IdentityServer4.EntityFramework.Entities.ApiScopeProperty
                        {
                            Key = prop.Key,
                            Value = prop.Value
                        }).ToList()
                    };

                    context.ApiScopes.Add(entity);
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    var entity = new IdentityServer4.EntityFramework.Entities.ApiResource
                    {
                        Name = resource.Name,
                        DisplayName = resource.DisplayName,
                        Description = resource.Description,
                        Enabled = resource.Enabled,
                        Scopes = resource.Scopes.Select(scope => new IdentityServer4.EntityFramework.Entities.ApiResourceScope
                        {
                            Scope = scope
                        }).ToList(),
                        UserClaims = resource.UserClaims.Select(claim => new IdentityServer4.EntityFramework.Entities.ApiResourceClaim
                        {
                            Type = claim
                        }).ToList(),
                        Secrets = resource.ApiSecrets.Select(secret => new IdentityServer4.EntityFramework.Entities.ApiResourceSecret
                        {
                            Type = secret.Type,
                            Value = secret.Value.Sha256()
                        }).ToList(),
                        Properties = resource.Properties.Select(prop => new IdentityServer4.EntityFramework.Entities.ApiResourceProperty
                        {
                            Key = prop.Key,
                            Value = prop.Value
                        }).ToList(),
                    };

                    context.ApiResources.Add(entity);
                }

                context.SaveChanges();
            }
        }
    }
}
