using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Notification.Notifications.Data;
using NouAssist.Notification.Notifications.Enums;
using NouAssist.Notification.Notifications.Events;
using Volo.Abp.Domain.Services;

namespace NouAssist.Notification.Notifications.Services
{
    public class NotificationDeliveryService : DomainService
    {
        private readonly INotificationPreferenceRepository _preferenceRepository;

        public NotificationDeliveryService(INotificationPreferenceRepository preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        public async Task<List<NotificationChannel>> GetEnabledChannelsAsync(
            Guid userId,
            NotificationType notificationType)
        {
            var preference = await _preferenceRepository.FindByUserAndTypeAsync(userId, notificationType);
            
            if (preference == null || !preference.IsActive)
            {
                // Return default channels
                return new List<NotificationChannel> { NotificationChannel.Push, NotificationChannel.Email };
            }

            var channels = new List<NotificationChannel>();
            
            if (preference.PushEnabled)
                channels.Add(NotificationChannel.Push);
            
            if (preference.EmailEnabled)
                channels.Add(NotificationChannel.Email);
            
            if (preference.SmsEnabled)
                channels.Add(NotificationChannel.Sms);

            return channels;
        }

        public void PrepareNotificationForDelivery(
            Notification notification,
            List<NotificationChannel> channels)
        {
            if (channels == null || channels.Count == 0)
            {
                notification.MarkAsFailed();
                return;
            }

            foreach (var channel in channels)
            {
                notification.AddDeliveryLog(channel, NotificationDeliveryStatus.Pending);
            }

            // Publish event for delivery
            notification.AddDistributedEvent(new NotificationSentEto
            {
                NotificationId = notification.Id,
                UserId = notification.UserId,
                NotificationType = notification.NotificationType.ToString(),
                Priority = notification.Priority.ToString(),
                TenantId = notification.TenantId,
                SentAt = Clock.Now
            });
        }

        public bool ShouldDelayDelivery(Notification notification)
        {
            // Don't delay urgent notifications
            if (notification.Priority == NotificationPriority.Urgent)
                return false;

            // Could implement quiet hours logic here
            var now = Clock.Now;
            var hour = now.Hour;
            
            // Delay non-urgent notifications during night hours (22:00 - 08:00)
            if (hour >= 22 || hour < 8)
                return notification.Priority != NotificationPriority.High;

            return false;
        }
    }
}
