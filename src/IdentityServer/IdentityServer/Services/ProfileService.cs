using IdentityServer.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = await _userManager.GetUserAsync(context.Subject);

            var claims = new List<Claim>
        {
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim("userName", user.Email),
            new Claim("email", user.Email),
            new Claim("id", user.Id),
        };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.IsActive;
        }
    }
}
