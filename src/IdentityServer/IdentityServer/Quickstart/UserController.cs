using IdentityModel;
using IdentityServer.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerHost.Quickstart.UI;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public async Task<IActionResult> UpdateProfile(User userModel)
    {
        // Retrieve the username from the JWT token
        var test1 = HttpContext.User.Claims;
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value;
        var userName = User.Identity?.Name;

        if (userName == null)
        {
            // Handling scenario when the username is not found in the token
            return Unauthorized();
        }

        // Find the user by username
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            // Handling scenario when the user is not found
            return NotFound();
        }

        // Update first name and last name
        user.FirstName = userModel.FirstName;
        user.LastName = userModel.LastName;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // If the update is successful, you can perform necessary actions, like redirecting to the profile page
            return Ok(user);
        }

        // Handling errors during the update process
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
                
        // If there are errors, return a BadRequest with the associated errors
        return BadRequest(ModelState);
    }
}