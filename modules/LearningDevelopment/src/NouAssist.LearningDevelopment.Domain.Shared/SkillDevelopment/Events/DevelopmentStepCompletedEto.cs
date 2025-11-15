using System;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SkillDevelopment.Events
{
    [EventName("NouAssist.SkillDevelopment.DevelopmentStepCompleted")]
    public class DevelopmentStepCompletedEto : IMultiTenant
    {
        public Guid SessionId { get; set; }
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DevelopmentStep CompletedStep { get; set; }
        public DateTime CompletedAt { get; set; }
        public int TimeSpentMinutes { get; set; }
    }
}
