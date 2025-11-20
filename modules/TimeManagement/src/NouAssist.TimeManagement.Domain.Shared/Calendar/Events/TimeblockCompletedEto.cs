using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TimeManagement.Calendar.Events
{
    [EventName("NouAssist.Calendar.TimeblockCompleted")]
    public class TimeblockCompletedEto : IMultiTenant
    {
        public Guid TimeblockId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CompletedAt { get; set; }
        public int DurationMinutes { get; set; }
        public int ActualDurationMinutes { get; set; }
        public string CompletionNotes { get; set; }

        // Linked entity information for integration
        public Guid? LinkedEntityId { get; set; }
        public string LinkedEntityType { get; set; }
    }
}
