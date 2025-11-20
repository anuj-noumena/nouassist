using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.Infrastructure.Notifications.Data;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace NouAssist.Infrastructure.Notifications.BackgroundWorkers
{
    public class ExpiredNotificationCleanupWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public ExpiredNotificationCleanupWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 3600000; // Run every 1 hour
        }

        [UnitOfWork]
        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("ExpiredNotificationCleanupWorker: Cleaning up expired notifications...");

            var notificationRepository = workerContext.ServiceProvider.GetRequiredService<INotificationRepository>();

            var expiredNotifications = await notificationRepository.GetExpiredNotificationsAsync();

            Logger.LogInformation("Found {Count} expired notifications", expiredNotifications.Count);

            // Delete in batch for better performance
            if (expiredNotifications.Any())
            {
                try
                {
                    await notificationRepository.DeleteManyAsync(expiredNotifications);
                    Logger.LogInformation("Deleted {Count} expired notifications", expiredNotifications.Count);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error deleting expired notifications");

                    // Fallback to individual deletion
                    foreach (var notification in expiredNotifications)
                    {
                        try
                        {
                            await notificationRepository.DeleteAsync(notification);
                            Logger.LogInformation("Deleted expired notification {NotificationId}", notification.Id);
                        }
                        catch (Exception deleteEx)
                        {
                            Logger.LogError(deleteEx, "Error deleting expired notification {NotificationId}", notification.Id);
                        }
                    }
                }
            }
        }
    }
}
