using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastInactiveDate { get; set; }
}