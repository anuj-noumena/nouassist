using System;
using NouAssist.Learning.Enums;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event triggered when a learning phase is stopped or completed
    /// Used to stop time tracking
    /// </summary>
    [Serializable]
    public class LearningPhaseStoppedEto
    {
        public Guid SessionId { get; set; }
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public LearningPhase Phase { get; set; }
        public Guid? TimeEntryId { get; set; }
        public int ActualMinutesSpent { get; set; }
        public DateTime StoppedAt { get; set; }
    }
}
