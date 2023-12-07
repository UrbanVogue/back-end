using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Services
{
    public class DeactivateUserBackgroundService
    {
        //private readonly UserManager<User> _userManager;

        //public DeactivateUserBackgroundService(UserManager<User> userManager, IUserStore<User> userStore)
        //{
        //    _userManager = userManager;
        //    _userStore = userStore;
        //}

        //public async Task DeactivateAccount()
        //{
        //    var inActiveUsers = _userManager.Users.Where(u => !u.IsActive);

        //    foreach (var user in inActiveUsers)
        //    {
        //        if (DateTime.UtcNow.Subtract(user.LastInactiveDate).TotalDays >= 14)
        //        {
        //            var result = await _userManager.DeleteAsync(user);

        //            if (!result.Succeeded)
        //            {
        //                // Handle delete failure
        //            }
        //        }
        //    }

        //}
    }
}
