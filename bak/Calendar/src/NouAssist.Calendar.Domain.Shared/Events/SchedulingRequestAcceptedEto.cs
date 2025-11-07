using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Calendar.Events
{
    [EventName("NouAssist.Calendar.SchedulingRequestAccepted")]
    public class SchedulingRequestAcceptedEto : IMultiTenant
    {
        public Guid RequestId { get; set; }
        public Guid RequesterId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid SelectedSlotId { get; set; }
        public Guid CalendarEventId { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
