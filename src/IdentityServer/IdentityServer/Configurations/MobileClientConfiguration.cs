using IdentityServer4.Models;

namespace IdentityServer.Configurations;

public class MobileClientConfiguration
{
    public ICollection<string> RedirectUris { get; set; }
    public ICollection<string> PostLogoutRedirectUris { get; set; }
    public ICollection<string> AllowedCorsOrigins { get; set; }
    public ICollection<Secret> ClientSecrets { get; set; }
}