using System;
using System.Collections.Generic;

namespace NouAssist.LearningDevelopment.Integration.Events
{
    /// <summary>
    /// Event requesting automatic creation of timeblocks for learning path
    /// </summary>
    [Serializable]
    public class RequestLearningTimeblocksEto
    {
        public Guid LearningPathId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public string Title { get; set; }
        public int EstimatedHours { get; set; }
        public List<LearningPhaseInfo> Phases { get; set; } = new();
        public PreferredSchedule Schedule { get; set; }
    }

    [Serializable]
    public class LearningPhaseInfo
    {
        public string PhaseName { get; set; }
        public int EstimatedMinutes { get; set; }
        public Guid SessionId { get; set; }
    }

    [Serializable]
    public class PreferredSchedule
    {
        public List<DayOfWeek> PreferredDays { get; set; } = new();
        public TimeSpan PreferredStartTime { get; set; }
        public int SessionDurationMinutes { get; set; }
    }
}
