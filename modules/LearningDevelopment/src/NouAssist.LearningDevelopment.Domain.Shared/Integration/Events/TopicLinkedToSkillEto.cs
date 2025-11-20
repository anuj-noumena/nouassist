using System;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event triggered when a Topic is linked to a Skill
    /// </summary>
    [Serializable]
    public class TopicLinkedToSkillEto
    {
        public Guid TopicId { get; set; }
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime LinkedAt { get; set; }
    }
}
