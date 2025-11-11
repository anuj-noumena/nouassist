using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Tasks.ValueObjects
{
    public class TimeTrackingInfo : ValueObject
    {
        public int ActualMinutes { get; private set; }
        public DateTime? LastTrackedAt { get; private set; }

        private TimeTrackingInfo() { }

        // Represents the default state (no time tracked)
        public static TimeTrackingInfo None => new() { ActualMinutes = 0, LastTrackedAt = null };

        // Factory method to return a NEW instance with added time
        public TimeTrackingInfo LogTime(int minutes)
        {
            return new TimeTrackingInfo
            {
                ActualMinutes = this.ActualMinutes + minutes,
                LastTrackedAt = DateTime.UtcNow
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ActualMinutes;
            yield return LastTrackedAt;
        }
    }
}