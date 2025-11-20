using System;

namespace NouAssist.LearningDevelopment.Integration.LearningIntegration.Dtos
{
    public class LearningActivitySnapshotDto
    {
        public Guid LearningActivityId { get; set; }
        public Guid LearningPathId { get; set; }
        public Guid UserId { get; set; }
        public int OverallProgress { get; set; }
        public double TaskCompletionRate { get; set; }
        public int TotalTasksCreated { get; set; }
        public int TotalTasksCompleted { get; set; }
        public int TheoryTasksCompleted { get; set; }
        public int PracticeTasksCompleted { get; set; }
        public int ScheduledMinutes { get; set; }
        public int ActualMinutesSpent { get; set; }
        public int RemainingMinutes { get; set; }
        public double TimeEfficiency { get; set; }
        public bool IsOnTrack { get; set; }
        public int TimeblockCount { get; set; }
        public int TimeEntryCount { get; set; }
        public DateTime? LastTaskCompletedAt { get; set; }
        public DateTime? LastTimeTrackedAt { get; set; }
        public DateTime LastSyncAt { get; set; }
    }
}
