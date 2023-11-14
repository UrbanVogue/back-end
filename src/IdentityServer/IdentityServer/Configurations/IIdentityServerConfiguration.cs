using IdentityServer4.Models;

namespace IdentityServer.Configurations;

public interface IIdentityServerConfiguration
{
    IEnumerable<Client> GetClients();
    IEnumerable<ApiScope> GetApiScopes();
    IEnumerable<ApiResource> GetApiResources();
    IEnumerable<IdentityResource> GetIdentityResources();
}