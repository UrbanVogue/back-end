using Bogus;
using IdentityModel;
using IdentityServer.Configurations;
using IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServer
{
    public class SeedData
    {
        public static async Task EnsureSeedDataAsync(IServiceProvider serviceProvider)
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

            var countUsers = userMgr.Users.Count();

            if (countUsers == 0)
            {
                List<string> userNames = new();
                for (var i = 0; i < 10; i++)
                    userNames.Add(new Faker().Internet.Email());

                foreach (var username in userNames)
                {
                    var user = await userMgr.FindByNameAsync(username);

                    if (user != null)
                    {
                        continue;
                    }
                    
                    user = new Faker<IdentityUser>()
                        .RuleFor(u => u.UserName, username)
                        .RuleFor(u => u.Email, username)
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

        private static async Task EnsureSeedConfigDataAsync(ConfigurationDbContext context)
        {
            var clientsIsExists = await context.Clients.AnyAsync();
            if (!clientsIsExists)
            {
                var clients = IdentityServerConfiguration.Clients
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
                var resources = IdentityServerConfiguration.IdentityResources
                    .ToList()
                    .Select(resource => resource.ToEntity());
                
                foreach (var entity in resources)
                {
                    await context.IdentityResources.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }

            if (!await context.ApiScopes.AnyAsync())
            {
                var apiScopes = IdentityServerConfiguration.ApiScopes
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
                var apiResources = IdentityServerConfiguration.ApiResources
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
