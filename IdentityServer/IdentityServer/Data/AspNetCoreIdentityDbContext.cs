
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class AspNetCoreIdentityDbContext : IdentityDbContext
    {
        public AspNetCoreIdentityDbContext(DbContextOptions<AspNetCoreIdentityDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
