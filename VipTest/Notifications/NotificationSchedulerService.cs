namespace VipTest.Notifications;

public class NotificationSchedulerService:BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationSchedulerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessScheduledNotifications(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task ProcessScheduledNotifications(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationTemplateServices>();
            await notificationService.SendScheduledNotificationsAsync();
        }
    }
}