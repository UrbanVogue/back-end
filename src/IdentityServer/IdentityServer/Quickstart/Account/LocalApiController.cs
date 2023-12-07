using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Quickstart.Account;

[Route("user")]
public class LocalApiController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly AspNetCoreIdentityDbContext _context;

    public LocalApiController(UserManager<User> userManager, AspNetCoreIdentityDbContext identityDbContext)
    {
        _context = identityDbContext;
        _userManager = userManager;
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateAccount()
    {
        var accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized();
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            var userEmail = jsonToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = false;
            user.LastInactiveDate = DateTime.UtcNow;
            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok();
            }

            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
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
                return Ok();
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

    [HttpPost("cleanup")]
    public async Task<IActionResult> CleanUpInactiveAccounts()
    {
        var users = await _context.Users.Where(u => !u.IsActive).ToListAsync();
        IEnumerable<User> inactiveUsers = users.Where(u => u.LastInactiveDate != null && DateTime.UtcNow.Subtract(u.LastInactiveDate.Value).TotalDays >= 14);

        foreach (var user in inactiveUsers)
        {
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                // Обробка невдалого видалення
                return BadRequest();
            }
        }

        return Ok();
    }
}