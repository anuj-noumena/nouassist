using System;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event triggered when the theory phase of a learning path is completed
    /// Signals readiness to start practice
    /// </summary>
    [Serializable]
    public class TheoryPhaseCompletedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid TopicId { get; set; }
        public Guid SkillId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public int TheoryMasteryScore { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
