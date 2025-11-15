using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Learning.Events
{
    [EventName("NouAssist.Learning.SessionStarted")]
    public class LearningSessionStartedEto : IMultiTenant
    {
        public Guid SessionId { get; set; }
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
