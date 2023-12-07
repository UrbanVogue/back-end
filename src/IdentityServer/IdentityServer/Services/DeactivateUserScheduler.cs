namespace IdentityServer.Services
{
    //public class DeactivateUserScheduler : BackgroundService
    //{
    //    private readonly IServiceScopeFactory _scopeFactory;

    //    public DeactivateUserScheduler(IServiceScopeFactory scopeFactory)
    //    {
    //        _scopeFactory = scopeFactory;
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await DeactivateUsers();
    //            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
    //        }
    //    }

    //    private async Task DeactivateUsers()
    //    {
    //        using (var scope = _scopeFactory.CreateScope())
    //        {
    //            var myService = scope.ServiceProvider.GetRequiredService<IMyService>();

    //            await myService.PerformDailyTask();
    //        }
    //    }
    //}
}
