using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Calendar.Events
{
    [EventName("NouAssist.Calendar.EventCancelled")]
    public class CalendarEventCancelledEto : IMultiTenant
    {
        public Guid EventId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CancelledAt { get; set; }
        public string CancelledBy { get; set; }
        public string CancellationReason { get; set; }
    }
}
