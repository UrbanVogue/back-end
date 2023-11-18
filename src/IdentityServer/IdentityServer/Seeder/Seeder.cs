using System.Security.Claims;
using Bogus;
using IdentityModel;
using IdentityServer.Configurations;
using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Seeder
{
    public class Seeder : ISeeder
    {
        private readonly IIdentityServerConfiguration _identityServerConfiguration;

        public Seeder(IIdentityServerConfiguration identityServerConfiguration)
        {
            _identityServerConfiguration = identityServerConfiguration;
        }

        public async Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
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

        private static async Task EnsureSeedUsersAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var countUsers = userMgr.Users.Count();

            if (countUsers == 0)
            {
                List<string> userNames = new() { "User", "Admin" };

                foreach (var username in userNames)
                {
                    if (await roleMgr.FindByNameAsync(username) == null)
                    {
                        await roleMgr.CreateAsync(new IdentityRole(username));
                    }

                    var user = await userMgr.FindByNameAsync(username);

                    if (user != null)
                    {
                        continue;
                    }

                    user = new Faker<IdentityUser>()
                        .RuleFor(u => u.UserName, username)
                        .RuleFor(u => u.Email, f => f.Internet.Email())
                        .RuleFor(u => u.EmailConfirmed, true).Generate();
                    var result = await userMgr.CreateAsync(user, "Pass123$");

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = await userMgr.AddToRoleAsync(user, username);

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = await userMgr.AddClaimsAsync(
                        user,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.GivenName, user.UserName),
                            new Claim(JwtClaimTypes.Name, user.UserName),
                            new Claim(JwtClaimTypes.Email, user.Email)
                        });

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }
            }
        }

        private async Task EnsureSeedConfigDataAsync(ConfigurationDbContext context)
        {
            var clientsIsExists = await context.Clients.AnyAsync();
            if (!clientsIsExists)
            {
                var clients = _identityServerConfiguration.GetClients()
                    .ToList()
                    .Select(client => client.ToEntity());
                
                foreach (var entity in clients)
                {
                    await context.Clients.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.IdentityResources.AnyAsync()))
            {
                var identityResources = _identityServerConfiguration.GetIdentityResources()
                    .ToList()
                    .Select(resource => resource.ToEntity());
                
                foreach (var entity in identityResources)
                {
                    await context.IdentityResources.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }

            if (!await context.ApiScopes.AnyAsync())
            {
                var apiScopes = _identityServerConfiguration.GetApiScopes()
                    .ToList()
                    .Select(resource => resource.ToEntity());
                
                foreach (var entity in apiScopes)
                {
                    await context.ApiScopes.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.ApiResources.AnyAsync()))
            {
                var apiResources = _identityServerConfiguration.GetApiResources()
                    .ToList()
                    .Select(resource => resource.ToEntity());
                
                foreach (var entity in apiResources)
                {
                    await context.ApiResources.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
