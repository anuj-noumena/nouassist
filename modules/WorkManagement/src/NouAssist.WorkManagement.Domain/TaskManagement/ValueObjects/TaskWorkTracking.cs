using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;
using Volo.Abp.Timing;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    /// <summary>
    /// Consolidated work tracking value object
    /// </summary>
    public class TaskWorkTracking : ValueObject
    {
        public double ProgressPercent { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public int ActualMinutes { get; private set; }
        public int? EstimatedMinutes { get; private set; }
        public bool IsNextAction { get; private set; }

        private TaskWorkTracking() { }

        public static TaskWorkTracking NotStarted(int? estimatedMinutes = null)
        {
            return new TaskWorkTracking
            {
                ProgressPercent = 0,
                EstimatedMinutes = estimatedMinutes,
                IsNextAction = false
            };
        }

        public TaskWorkTracking Start()
        {
            return new TaskWorkTracking
            {
                ProgressPercent = this.ProgressPercent,
                StartedAt = DateTime.UtcNow,
                CompletedAt = this.CompletedAt,
                ActualMinutes = this.ActualMinutes,
                EstimatedMinutes = this.EstimatedMinutes,
                IsNextAction = false
            };
        }

        public TaskWorkTracking Complete()
        {
            return new TaskWorkTracking
            {
                ProgressPercent = 100,
                StartedAt = this.StartedAt,
                CompletedAt = DateTime.UtcNow,
                ActualMinutes = this.ActualMinutes,
                EstimatedMinutes = this.EstimatedMinutes,
                IsNextAction = false
            };
        }

        public TaskWorkTracking UpdateProgress(double percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percent));
            }

            return new TaskWorkTracking
            {
                ProgressPercent = percent,
                StartedAt = this.StartedAt,
                CompletedAt = this.CompletedAt,
                ActualMinutes = this.ActualMinutes,
                EstimatedMinutes = this.EstimatedMinutes,
                IsNextAction = this.IsNextAction
            };
        }

        public TaskWorkTracking LogTime(int minutes)
        {
            return new TaskWorkTracking
            {
                ProgressPercent = this.ProgressPercent,
                StartedAt = this.StartedAt,
                CompletedAt = this.CompletedAt,
                ActualMinutes = this.ActualMinutes + minutes,
                EstimatedMinutes = this.EstimatedMinutes,
                IsNextAction = this.IsNextAction
            };
        }

        public TaskWorkTracking WithEstimate(int minutes)
        {
            return new TaskWorkTracking
            {
                ProgressPercent = this.ProgressPercent,
                StartedAt = this.StartedAt,
                CompletedAt = this.CompletedAt,
                ActualMinutes = this.ActualMinutes,
                EstimatedMinutes = minutes,
                IsNextAction = this.IsNextAction
            };
        }

        public TaskWorkTracking MarkAsNextAction()
        {
            return new TaskWorkTracking
            {
                ProgressPercent = this.ProgressPercent,
                StartedAt = this.StartedAt,
                CompletedAt = this.CompletedAt,
                ActualMinutes = this.ActualMinutes,
                EstimatedMinutes = this.EstimatedMinutes,
                IsNextAction = true
            };
        }

        public TaskWorkTracking UnmarkAsNextAction()
        {
            return new TaskWorkTracking
            {
                ProgressPercent = this.ProgressPercent,
                StartedAt = this.StartedAt,
                CompletedAt = this.CompletedAt,
                ActualMinutes = this.ActualMinutes,
                EstimatedMinutes = this.EstimatedMinutes,
                IsNextAction = false
            };
        }

        public TaskWorkTracking UpdateFlag(bool isFlagged, string flagReason = null)
        {
            // Note: Flag reason is stored in TaskFlowControlAggregate, not in WorkTracking
            // This method is here for backwards compatibility but doesn't actually handle the flag
            return this;
        }

        // Computed properties
        public bool HasStarted => StartedAt.HasValue;
        public bool IsCompleted => CompletedAt.HasValue;
        public bool IsInProgress => HasStarted && !IsCompleted;
        public TimeSpan? Duration => CompletedAt.HasValue && StartedAt.HasValue
            ? CompletedAt.Value - StartedAt.Value
            : null;
        public bool IsOverEstimate => EstimatedMinutes.HasValue && 
                                      ActualMinutes > EstimatedMinutes.Value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ProgressPercent;
            yield return StartedAt;
            yield return CompletedAt;
            yield return ActualMinutes;
            yield return EstimatedMinutes;
            yield return IsNextAction;
        }
    }
}