using System;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event triggered when a Skill is linked to a Topic
    /// </summary>
    [Serializable]
    public class SkillLinkedToTopicEto
    {
        public Guid SkillId { get; set; }
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime LinkedAt { get; set; }
    }
}
