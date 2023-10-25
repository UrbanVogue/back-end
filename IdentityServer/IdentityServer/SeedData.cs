using Bogus;
using IdentityModel;
using IdentityServer.Configurations;
using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer
{
    public class SeedData
    {
        public async static Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                await persistedGrantDbContext.Database.MigrateAsync();

                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                await configurationDbContext.Database.MigrateAsync();
                var clientsExist = await configurationDbContext.Clients.AnyAsync();
                if (!clientsExist)
                {
                    await EnsureSeedConfigDataAsync(configurationDbContext);
                }

                var aspNetCoreIdentityDbContext = scope.ServiceProvider.GetRequiredService<AspNetCoreIdentityDbContext>();
                await aspNetCoreIdentityDbContext.Database.MigrateAsync();
                await EnsureSeedUsersAsync(serviceProvider);
            }
        }

        private async static Task EnsureSeedUsersAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var countUsers = userMgr.Users.Count();

                if (countUsers == 0)
                {
                    List<string> userNames = new();
                    for (int i = 0; i < 10; i++)
                        userNames.Add(new Faker().Internet.UserName());

                    foreach (var username in userNames)
                    {
                        var user = await userMgr.FindByNameAsync(username);
                        if (user == null)
                        {
                            user = new Faker<IdentityUser>()
                                .RuleFor(u => u.UserName, username)
                                .RuleFor(u => u.Email, f => f.Internet.Email(username))
                                .RuleFor(u => u.EmailConfirmed, true).Generate();

                            var result = await userMgr.CreateAsync(user, "Pass123$");

                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = await userMgr.AddClaimsAsync(
                                    user,
                                    new Claim[]
                                    {
                                    new Claim(JwtClaimTypes.GivenName, new Faker().Name.FirstName()),
                                    new Claim(JwtClaimTypes.FamilyName, new Faker().Name.LastName())
                                    });

                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }
                    }
                }
            }
        }

        private async static Task EnsureSeedConfigDataAsync(ConfigurationDbContext context)
        {
            if (!(await context.Clients.AnyAsync()))
            {
                foreach (var client in IdentityServerConfiguration.Clients.ToList())
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
                    await context.Clients.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.IdentityResources.AnyAsync()))
            {
                foreach (var resource in IdentityServerConfiguration.IdentityResources.ToList())
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

                    await context.IdentityResources.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.ApiScopes.AnyAsync()))
            {
                foreach (var resource in IdentityServerConfiguration.ApiScopes.ToList())
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

                    await context.ApiScopes.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.ApiResources.AnyAsync()))
            {
                foreach (var resource in IdentityServerConfiguration.ApiResources.ToList())
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

                    await context.ApiResources.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
