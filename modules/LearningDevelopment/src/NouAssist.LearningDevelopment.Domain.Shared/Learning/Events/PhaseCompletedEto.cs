using System;
using NouAssist.Learning.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Learning.Events
{
    [EventName("NouAssist.Learning.PhaseCompleted")]
    public class PhaseCompletedEto : IMultiTenant
    {
        public Guid SessionId { get; set; }
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public LearningPhase Phase { get; set; }
        public int TimeSpentMs { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
