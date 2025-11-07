using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.TimeManagement.Events
{
    [EventName("NouAssist.CoreProductivity.TimeManagement.EventCreated")]
    public class CalendarEventCreatedEto : IMultiTenant
    {
        public Guid EventId { get; set; }
        public Guid CalendarId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? TenantId { get; set; }
        public string Title { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsRecurring { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
