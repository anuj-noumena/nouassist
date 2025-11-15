using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Learning.Events
{
    [EventName("NouAssist.Learning.TopicMastered")]
    public class TopicMasteredEto : IMultiTenant
    {
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid SessionId { get; set; }
        public int TotalTimeSpentMs { get; set; }
        public DateTime MasteredAt { get; set; }
    }
}
