using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.TimeManagement.Events
{
    [EventName("NouAssist.CoreProductivity.TimeManagement.EventCancelled")]
    public class CalendarEventCancelledEto : IMultiTenant
    {
        public Guid EventId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CancelledAt { get; set; }
        public string CancelledBy { get; set; }
        public string CancellationReason { get; set; }
    }
}
