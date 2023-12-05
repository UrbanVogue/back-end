using IdentityModel;
using IdentityServer.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Quickstart.Account;

[Route("user")]
public class LocalApiController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public LocalApiController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] User userModel)
    {
        // Retrieve the access token from the Authorization header
        var accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(accessToken))
        {
            // Token not found or invalid
            return Unauthorized();
        }

        try
        {
            // Decode the token to extract user claims
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            // Get the user's email claim
            var userEmail = jsonToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                // Email not found in claims
                return Unauthorized();
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                // User not found
                return NotFound();
            }

            // Update user details
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // If the update is successful, return the updated user
                return Ok(user.Id);
            }

            // Handling errors during the update process
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If there are errors, return a BadRequest with the associated errors
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            // Exception handling (token decoding error, etc.)
            return StatusCode(500, "Internal server error");
        }
    }

}