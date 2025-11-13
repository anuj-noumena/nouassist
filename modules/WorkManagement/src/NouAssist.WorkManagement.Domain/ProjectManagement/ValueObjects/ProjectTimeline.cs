using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;
using Volo.Abp.Timing;

namespace NouAssist.WorkManagement.ProjectManagement.ValueObjects
{
    /// <summary>
    /// Project timeline value object
    /// </summary>
    public class ProjectTimeline : ValueObject
    {
        public DateTime? PlannedStartDate { get; private set; }
        public DateTime? PlannedDueDate { get; private set; }
        public DateTime? ActualStartDate { get; private set; }
        public DateTime? ActualCompletedDate { get; private set; }
        
        private ProjectTimeline() { }
        
        public static ProjectTimeline Create(DateTime? plannedStart, DateTime? plannedDue)
        {
            if (plannedStart.HasValue && plannedDue.HasValue && plannedDue < plannedStart)
            {
                throw new BusinessException("Projects:InvalidDateRange");
            }
            
            return new ProjectTimeline
            {
                PlannedStartDate = plannedStart,
                PlannedDueDate = plannedDue
            };
        }
        
        public ProjectTimeline Update(DateTime? plannedStart, DateTime? plannedDue)
        {
            if (plannedStart.HasValue && plannedDue.HasValue && plannedDue < plannedStart)
            {
                throw new BusinessException("Projects:InvalidDateRange");
            }
            
            return new ProjectTimeline
            {
                PlannedStartDate = plannedStart,
                PlannedDueDate = plannedDue,
                ActualStartDate = this.ActualStartDate,
                ActualCompletedDate = this.ActualCompletedDate
            };
        }
        
        public ProjectTimeline MarkStarted(DateTime startDate)
        {
            return new ProjectTimeline
            {
                PlannedStartDate = this.PlannedStartDate,
                PlannedDueDate = this.PlannedDueDate,
                ActualStartDate = startDate,
                ActualCompletedDate = this.ActualCompletedDate
            };
        }
        
        public ProjectTimeline MarkCompleted(DateTime completedDate)
        {
            return new ProjectTimeline
            {
                PlannedStartDate = this.PlannedStartDate,
                PlannedDueDate = this.PlannedDueDate,
                ActualStartDate = this.ActualStartDate,
                ActualCompletedDate = completedDate
            };
        }
        
        public bool HasStarted => ActualStartDate.HasValue;
        public bool IsCompleted => ActualCompletedDate.HasValue;
        public bool IsOverdue => PlannedDueDate.HasValue && 
                                  DateTime.UtcNow > PlannedDueDate.Value && 
                                  !IsCompleted;
        
        public TimeSpan? TimeRemaining => PlannedDueDate.HasValue && !IsCompleted
            ? PlannedDueDate.Value - DateTime.UtcNow
            : null;
        
        public double TimeProgress
        {
            get
            {
                if (!PlannedStartDate.HasValue || !PlannedDueDate.HasValue)
                {
                    return 0;
                }

                var totalDuration = (PlannedDueDate.Value - PlannedStartDate.Value).TotalDays;
                var elapsed = (DateTime.UtcNow - PlannedStartDate.Value).TotalDays;
                
                return Math.Clamp(elapsed / totalDuration, 0, 1);
            }
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PlannedStartDate;
            yield return PlannedDueDate;
            yield return ActualStartDate;
            yield return ActualCompletedDate;
        }
    }
}