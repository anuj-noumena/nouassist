using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Notifications.Data;
using NouAssist.Notification.Notifications.Enums;
using NouAssist.Notification.Notifications.Events;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.Infrastructure.Notifications.Services
{
    public class NotificationDeliveryService : DomainService
    {
        private readonly INotificationPreferenceRepository _preferenceRepository;
        private readonly IDistributedEventBus _distributedEventBus;

        public NotificationDeliveryService(INotificationPreferenceRepository preferenceRepository, IDistributedEventBus distributedEventBus)
        {
            _preferenceRepository = preferenceRepository;
            _distributedEventBus = distributedEventBus;
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
            {
                channels.Add(NotificationChannel.Push);
            }

            if (preference.EmailEnabled)
            {
                channels.Add(NotificationChannel.Email);
            }

            if (preference.SmsEnabled)
            {
                channels.Add(NotificationChannel.Sms);
            }

            return channels;
        }

        public void PrepareNotificationForDelivery(
            Aggregates.Notification notification,
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
            _distributedEventBus.PublishAsync(new NotificationSentEto
            {
                NotificationId = notification.Id,
                UserId = notification.UserId,
                NotificationType = notification.NotificationType.ToString(),
                Priority = notification.Priority.ToString(),
                TenantId = notification.TenantId,
                SentAt = Clock.Now
            });
        }

        public bool ShouldDelayDelivery(Aggregates.Notification notification)
        {
            // Don't delay urgent notifications
            if (notification.Priority == NotificationPriority.Urgent)
            {
                return false;
            }

            // Could implement quiet hours logic here
            var now = Clock.Now;
            var hour = now.Hour;

            // Delay non-urgent notifications during night hours (22:00 - 08:00)
            if (hour >= 22 || hour < 8)
            {
                return notification.Priority != NotificationPriority.High;
            }

            return false;
        }

        public async Task DeliverViaChannelAsync(Aggregates.Notification notification, string channelString)
        {
            // Parse channel string to enum
            if (!Enum.TryParse<NotificationChannel>(channelString, true, out var channel))
            {
                throw new ArgumentException($"Invalid notification channel: {channelString}", nameof(channelString));
            }

            // Find the delivery log for this channel
            var deliveryLog = notification.DeliveryLogs
                .Where(log => log.Channel == channel && log.IsPending)
                .OrderByDescending(log => log.AttemptedAt)
                .FirstOrDefault();

            if (deliveryLog == null)
            {
                // No pending delivery log found for this channel
                return;
            }

            try
            {
                // TODO: Implement actual delivery logic here
                // For now, we'll simulate successful delivery
                // In a real implementation, this would:
                // - Call email service for Email channel
                // - Call SMS service for SMS channel
                // - Call push notification service for Push channel

                await Task.CompletedTask; // Placeholder for actual async delivery

                // Mark the delivery log as delivered
                deliveryLog.MarkAsDelivered();

                // Check if all delivery logs are now delivered
                var allDelivered = notification.DeliveryLogs.All(log => log.WasDelivered);
                if (allDelivered)
                {
                    notification.MarkAsDelivered();
                }

                // Publish delivery success event
                await _distributedEventBus.PublishAsync(new NotificationDeliveredEto
                {
                    NotificationId = notification.Id,
                    UserId = notification.UserId,
                    Channel = channel.ToString(),
                    DeliveredAt = Clock.Now,
                    TenantId = notification.TenantId
                });
            }
            catch (Exception ex)
            {
                // Mark the delivery log as failed
                deliveryLog.MarkAsFailed(ex.Message);

                // Check if all delivery attempts have failed
                var allFailed = notification.DeliveryLogs.All(log => log.HasFailed);
                if (allFailed)
                {
                    notification.MarkAsFailed();
                }

                // Publish delivery failure event
                await _distributedEventBus.PublishAsync(new NotificationDeliveryFailedEto
                {
                    NotificationId = notification.Id,
                    UserId = notification.UserId,
                    Channel = channel.ToString(),
                    ErrorMessage = ex.Message,
                    FailedAt = Clock.Now,
                    TenantId = notification.TenantId
                });

                throw;
            }
        }
    }
}
