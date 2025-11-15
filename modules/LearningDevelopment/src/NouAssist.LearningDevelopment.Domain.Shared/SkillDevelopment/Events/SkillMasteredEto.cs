using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SkillDevelopment.Events
{
    [EventName("NouAssist.SkillDevelopment.SkillMastered")]
    public class SkillMasteredEto : IMultiTenant
    {
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string SkillTitle { get; set; }
        public DateTime MasteredAt { get; set; }
        public int FinalProficiencyScore { get; set; }
        public int TotalPracticeHours { get; set; }
    }
}
