using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.Reviews.Events
{
    [EventName("NouAssist.Reviews.Completed")]
    public class ReviewCompletedEto : IMultiTenant
    {
        public Guid ReviewId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime OccurredAt { get; set; }
        public int? CompletenessScore { get; set; }
        public string ReviewType { get; set; }
    }
}
