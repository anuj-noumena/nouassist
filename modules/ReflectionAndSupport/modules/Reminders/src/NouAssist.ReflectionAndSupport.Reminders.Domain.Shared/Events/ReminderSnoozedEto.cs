using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.ReflectionAndSupport.Reminders.Events
{
    [EventName("NouAssist.ReflectionAndSupport.Reminders.Snoozed")]
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
