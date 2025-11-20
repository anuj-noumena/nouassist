using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Notification.Reminders.Events
{
    [EventName("NouAssist.Reminders.Snoozed")]
    public class ReminderSnoozedEto : IMultiTenant
    {
        public Guid ReminderId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime OriginalRemindAt { get; set; }
        public DateTime NewRemindAt { get; set; }
        public int SnoozeDurationMinutes { get; set; }
        public Guid? TenantId { get; set; }
    }
}
