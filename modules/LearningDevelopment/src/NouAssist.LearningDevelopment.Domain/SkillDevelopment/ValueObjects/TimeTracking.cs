using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects
{
    public class TimeTracking : ValueObject
    {
        public int TotalMinutes { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private TimeTracking() { /* ORM */ }

        public TimeTracking(DateTime startedAt)
        {
            StartedAt = startedAt;
            TotalMinutes = 0;
        }

        public void AddTime(int minutes)
        {
            if (minutes < 0) throw new ArgumentException("Minutes cannot be negative", nameof(minutes));
            TotalMinutes += minutes;
        }

        public void Complete(DateTime completedAt)
        {
            if (completedAt < StartedAt)
                throw new ArgumentException("Completion time cannot be before start time");
            CompletedAt = completedAt;
        }

        public int GetTotalHours() => TotalMinutes / 60;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TotalMinutes;
            yield return StartedAt;
            yield return CompletedAt;
        }
    }
}
