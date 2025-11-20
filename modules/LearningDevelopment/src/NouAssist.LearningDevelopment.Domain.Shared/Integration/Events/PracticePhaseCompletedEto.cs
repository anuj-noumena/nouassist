using System;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event triggered when the practice phase of a learning path is completed
    /// </summary>
    [Serializable]
    public class PracticePhaseCompletedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid TopicId { get; set; }
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public int PracticeProficiencyScore { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
