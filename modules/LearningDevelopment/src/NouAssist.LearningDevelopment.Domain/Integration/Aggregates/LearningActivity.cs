using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.Integration.Aggregates
{
    /// <summary>
    /// Aggregate that provides a unified view of all learning-related activities
    /// Tracks progress across LearningDevelopment, TaskManagement, and TimeManagement
    /// </summary>
    public class LearningActivity : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public Guid LearningPathId { get; private set; }

        // References to related entities
        private readonly List<Guid> _taskInstanceIds = new();
        public IReadOnlyCollection<Guid> TaskInstanceIds => _taskInstanceIds.AsReadOnly();

        private readonly List<Guid> _timeblockIds = new();
        public IReadOnlyCollection<Guid> TimeblockIds => _timeblockIds.AsReadOnly();

        private readonly List<Guid> _timeEntryIds = new();
        public IReadOnlyCollection<Guid> TimeEntryIds => _timeEntryIds.AsReadOnly();

        // Task metrics
        public int TotalTasksCreated { get; private set; }
        public int TheoryTasksCompleted { get; private set; }
        public int PracticeTasksCompleted { get; private set; }
        public int TotalTasksCompleted => TheoryTasksCompleted + PracticeTasksCompleted;
        public double TaskCompletionRate => TotalTasksCreated > 0
            ? (double)TotalTasksCompleted / TotalTasksCreated * 100
            : 0;

        // Time metrics
        public int ScheduledMinutes { get; private set; }
        public int ActualMinutesSpent { get; private set; }
        public int RemainingMinutes => Math.Max(0, ScheduledMinutes - ActualMinutesSpent);
        public double TimeEfficiency => ScheduledMinutes > 0
            ? (double)ActualMinutesSpent / ScheduledMinutes * 100
            : 0;

        // Progress metrics
        public int OverallProgress { get; private set; }
        public bool IsOnTrack => TimeEfficiency <= 120; // Within 20% buffer

        // Activity tracking
        public DateTime? LastTaskCompletedAt { get; private set; }
        public DateTime? LastTimeTrackedAt { get; private set; }
        public DateTime LastSyncAt { get; private set; }

        protected LearningActivity() { }

        public LearningActivity(
            Guid id,
            Guid userId,
            Guid learningPathId,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = userId;
            LearningPathId = learningPathId;
            TenantId = tenantId;
            TotalTasksCreated = 0;
            TheoryTasksCompleted = 0;
            PracticeTasksCompleted = 0;
            ScheduledMinutes = 0;
            ActualMinutesSpent = 0;
            OverallProgress = 0;
            LastSyncAt = DateTime.UtcNow;
        }

        public void AssociateTasks(List<Guid> taskInstanceIds, int theoryCount, int practiceCount)
        {
            _taskInstanceIds.AddRange(taskInstanceIds);
            TotalTasksCreated = theoryCount + practiceCount;
            LastSyncAt = DateTime.UtcNow;
        }

        public void AssociateTimeblocks(List<Guid> timeblockIds, int totalScheduledMinutes)
        {
            _timeblockIds.AddRange(timeblockIds);
            ScheduledMinutes = totalScheduledMinutes;
            LastSyncAt = DateTime.UtcNow;
        }

        public void RecordTaskCompletion(Guid taskInstanceId, bool isTheoryTask)
        {
            if (isTheoryTask)
            {
                TheoryTasksCompleted++;
            }
            else
            {
                PracticeTasksCompleted++;
            }

            LastTaskCompletedAt = DateTime.UtcNow;
            LastSyncAt = DateTime.UtcNow;
            RecalculateProgress();
        }

        public void RecordTimeEntry(Guid timeEntryId, int durationMinutes)
        {
            if (!_timeEntryIds.Contains(timeEntryId))
            {
                _timeEntryIds.Add(timeEntryId);
            }

            ActualMinutesSpent += durationMinutes;
            LastTimeTrackedAt = DateTime.UtcNow;
            LastSyncAt = DateTime.UtcNow;
            RecalculateProgress();
        }

        public void SyncFromLearningPath(int learningPathProgress)
        {
            OverallProgress = learningPathProgress;
            LastSyncAt = DateTime.UtcNow;
        }

        private void RecalculateProgress()
        {
            // Calculate weighted progress
            // 50% from task completion, 50% from time investment
            var taskProgress = TaskCompletionRate * 0.5;
            var timeProgress = Math.Min(100, TimeEfficiency) * 0.5;
            OverallProgress = (int)(taskProgress + timeProgress);
        }

        public LearningActivitySnapshot GetSnapshot()
        {
            return new LearningActivitySnapshot
            {
                LearningActivityId = Id,
                LearningPathId = LearningPathId,
                UserId = UserId,
                OverallProgress = OverallProgress,
                TaskCompletionRate = TaskCompletionRate,
                TotalTasksCreated = TotalTasksCreated,
                TotalTasksCompleted = TotalTasksCompleted,
                TheoryTasksCompleted = TheoryTasksCompleted,
                PracticeTasksCompleted = PracticeTasksCompleted,
                ScheduledMinutes = ScheduledMinutes,
                ActualMinutesSpent = ActualMinutesSpent,
                RemainingMinutes = RemainingMinutes,
                TimeEfficiency = TimeEfficiency,
                IsOnTrack = IsOnTrack,
                TimeblockCount = _timeblockIds.Count,
                TimeEntryCount = _timeEntryIds.Count,
                LastTaskCompletedAt = LastTaskCompletedAt,
                LastTimeTrackedAt = LastTimeTrackedAt,
                LastSyncAt = LastSyncAt
            };
        }
    }

    public class LearningActivitySnapshot
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
