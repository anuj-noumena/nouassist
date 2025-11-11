using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Notifications.Data;
using NouAssist.Notifications.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.Notifications.BackgroundJobs
{
    public class NotificationDeliveryJobArgs
    {
        public Guid NotificationId { get; set; }
        public string Channel { get; set; }
    }

    public class NotificationDeliveryJob : AsyncBackgroundJob<NotificationDeliveryJobArgs>, ITransientDependency
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationDeliveryService _deliveryService;

        public NotificationDeliveryJob(
            INotificationRepository notificationRepository,
            NotificationDeliveryService deliveryService)
        {
            _notificationRepository = notificationRepository;
            _deliveryService = deliveryService;
        }

        [UnitOfWork]
        public override async Task ExecuteAsync(NotificationDeliveryJobArgs args)
        {
            Logger.LogInformation($"Delivering notification {args.NotificationId} via {args.Channel}");

            try
            {
                var notification = await _notificationRepository.GetWithDeliveryLogsAsync(args.NotificationId);

                if (notification.IsDeleted)
                {
                    Logger.LogInformation($"Notification {args.NotificationId} is deleted");
                    return;
                }

                // Deliver via domain service
                await _deliveryService.DeliverViaChannelAsync(notification, args.Channel);

                await _notificationRepository.UpdateAsync(notification);

                Logger.LogInformation($"Successfully delivered notification {args.NotificationId} via {args.Channel}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error delivering notification {args.NotificationId} via {args.Channel}");
                throw;
            }
        }
    }
}
