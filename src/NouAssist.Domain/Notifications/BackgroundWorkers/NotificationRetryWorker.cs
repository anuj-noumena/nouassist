using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.Notifications.BackgroundJobs;
using NouAssist.Notifications.Data;
using NouAssist.Notifications.Enums;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace NouAssist.Notifications.BackgroundWorkers
{
    public class NotificationRetryWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public NotificationRetryWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 300000; // Run every 5 minutes
        }

        [UnitOfWork]
        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("NotificationRetryWorker: Retrying failed notifications...");

            var notificationRepository = workerContext.ServiceProvider.GetRequiredService<INotificationRepository>();
            var backgroundJobManager = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobManager>();

            // Get notifications with failed delivery
            var spec = new Specification<Aggregates.Notification>(n => 
                n.DeliveryStatus == NotificationDeliveryStatus.Failed &&
                n.CreationTime > DateTime.UtcNow.AddHours(-24) && // Only retry within 24 hours
                !n.IsDeleted
            );

            var failedNotifications = await notificationRepository.GetListAsync(spec, 50);

            Logger.LogInformation($"Found {failedNotifications.Count} failed notifications to retry");

            foreach (var notification in failedNotifications)
            {
                try
                {
                    // Get failed channels
                    var failedChannels = notification.DeliveryLogs
                        .Where(log => log.Status == NotificationDeliveryStatus.Failed)
                        .Select(log => log.Channel.ToString())
                        .Distinct();

                    foreach (var channel in failedChannels)
                    {
                        // Queue retry job
                        await backgroundJobManager.EnqueueAsync(
                            new NotificationDeliveryJobArgs 
                            { 
                                NotificationId = notification.Id,
                                Channel = channel
                            }
                        );

                        Logger.LogInformation($"Queued retry job for notification {notification.Id} via {channel}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Error queuing retry for notification {notification.Id}");
                }
            }
        }
    }
}
