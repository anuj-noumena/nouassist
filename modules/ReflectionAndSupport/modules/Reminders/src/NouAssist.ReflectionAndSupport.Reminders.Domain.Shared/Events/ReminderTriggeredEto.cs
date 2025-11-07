using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.ReflectionAndSupport.Reminders.Events
{
    [EventName("NouAssist.ReflectionAndSupport.Reminders.Triggered")]
    public class ReminderTriggeredEto : IMultiTenant
    {
        public Guid ReminderId { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string Priority { get; set; }
        public DateTime RemindAt { get; set; }
        public DateTime TriggeredAt { get; set; }
        public Guid? TenantId { get; set; }
    }
}
