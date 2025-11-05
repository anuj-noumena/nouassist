using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.Notifications.Data;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace NouAssist.Notifications.BackgroundWorkers
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

            Logger.LogInformation($"Found {expiredNotifications.Count} expired notifications");

            foreach (var notification in expiredNotifications)
            {
                try
                {
                    notification.Delete(Guid.Empty.ToString()); // System deletion
                    await notificationRepository.UpdateAsync(notification);
                    Logger.LogInformation($"Deleted expired notification: {notification.Id}");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Error deleting expired notification {notification.Id}");
                }
            }
        }
    }
}
