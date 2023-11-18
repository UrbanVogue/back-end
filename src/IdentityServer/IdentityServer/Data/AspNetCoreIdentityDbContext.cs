
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class AspNetCoreIdentityDbContext : IdentityDbContext<User>
    {
        public AspNetCoreIdentityDbContext(DbContextOptions<AspNetCoreIdentityDbContext> options) : base(options) 
        {
        }
    }
}
