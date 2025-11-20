using System;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event triggered when both theory and practice are mastered
    /// </summary>
    [Serializable]
    public class IntegratedMasteryAchievedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid TopicId { get; set; }
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public int IntegratedMasteryScore { get; set; }
        public int TheoryScore { get; set; }
        public int PracticeScore { get; set; }
        public DateTime AchievedAt { get; set; }
    }
}
