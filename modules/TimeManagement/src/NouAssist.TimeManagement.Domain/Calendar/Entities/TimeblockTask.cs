using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.TimeManagement.Calendar.Entities
{
    public class TimeblockTask : Entity<Guid>
    {
        public Guid TimeblockId { get; private set; }
        public Guid TaskInstanceId { get; private set; }
        public int EstimatedMinutes { get; private set; }
        public int ActualMinutes { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected TimeblockTask() { }

        public TimeblockTask(
            Guid id,
            Guid timeblockId,
            Guid taskInstanceId,
            int estimatedMinutes = 0)
            : base(id)
        {
            TimeblockId = timeblockId;
            TaskInstanceId = taskInstanceId;
            EstimatedMinutes = estimatedMinutes;
            ActualMinutes = 0;
            IsCompleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (IsCompleted)
                return;

            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }

        public void LogTime(int minutes)
        {
            ActualMinutes += minutes;
        }

        public void UpdateEstimate(int minutes)
        {
            EstimatedMinutes = minutes;
        }

        public bool HasEstimate => EstimatedMinutes > 0;
        public bool HasActualTime => ActualMinutes > 0;
        
        public double TimeVariance
        {
            get
            {
                if (EstimatedMinutes == 0) return 0.0;
                return ((ActualMinutes - EstimatedMinutes) / (double)EstimatedMinutes) * 100;
            }
        }
    }
}
