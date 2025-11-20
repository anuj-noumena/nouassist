using System.Threading.Tasks;
using NouAssist.Infrastructure.Reminders.Aggregates;
using NouAssist.Notification.Notifications.Enums;
using NouAssist.Notification.Reminders.Events;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;

namespace NouAssist.Infrastructure.Reminders.Services
{
    public class ReminderNotificationService : DomainService
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public ReminderNotificationService(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task SendReminderNotificationAsync(Reminder reminder)
        {
            if (reminder.IsCancelled)
            {
                return;
            }

            // Determine notification priority based on reminder priority
            var notificationPriority = MapReminderPriorityToNotificationPriority(reminder.Priority);

            // Publish event that will trigger notification creation
            await _distributedEventBus.PublishAsync(new ReminderTriggeredEto
            {
                ReminderId = reminder.Id,
                OwnerId = reminder.OwnerId,
                Title = reminder.Title,
                Priority = reminder.Priority.ToString(),
                EntityType = reminder.RefEntityType?.ToString(),
                EntityId = reminder.RefEntityId,
                RemindAt = reminder.RemindAt,
                TenantId = reminder.TenantId,
                TriggeredAt = Clock.Now
            });
        }

        private NotificationPriority MapReminderPriorityToNotificationPriority(Notification.Reminders.Enums.ReminderPriority reminderPriority)
        {
            return reminderPriority switch
            {
                Notification.Reminders.Enums.ReminderPriority.Critical => NotificationPriority.Urgent,
                Notification.Reminders.Enums.ReminderPriority.High => NotificationPriority.High,
                Notification.Reminders.Enums.ReminderPriority.Medium => NotificationPriority.Normal,
                Notification.Reminders.Enums.ReminderPriority.Normal => NotificationPriority.Normal,
                Notification.Reminders.Enums.ReminderPriority.Low => NotificationPriority.Low,
                _ => NotificationPriority.Normal
            };
        }
    }
}
