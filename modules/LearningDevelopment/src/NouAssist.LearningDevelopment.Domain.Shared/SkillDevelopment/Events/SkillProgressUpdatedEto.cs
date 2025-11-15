using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.SkillDevelopment.Events
{
    [EventName("NouAssist.SkillDevelopment.SkillProgressUpdated")]
    public class SkillProgressUpdatedEto : IMultiTenant
    {
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public int PreviousProficiency { get; set; }
        public int CurrentProficiency { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
