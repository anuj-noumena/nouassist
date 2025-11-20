using System;
using NouAssist.Learning.Enums;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event triggered when a learning phase is started
    /// Used to initiate time tracking
    /// </summary>
    [Serializable]
    public class LearningPhaseStartedEto
    {
        public Guid SessionId { get; set; }
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public LearningPhase Phase { get; set; }
        public string TopicTitle { get; set; }
        public bool RequestTimeTracking { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
