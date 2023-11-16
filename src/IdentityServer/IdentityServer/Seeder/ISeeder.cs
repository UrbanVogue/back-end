namespace IdentityServer.Seeder;

public interface ISeeder
{
    Task EnsureSeedDataAsync(IServiceProvider serviceProvider);
}