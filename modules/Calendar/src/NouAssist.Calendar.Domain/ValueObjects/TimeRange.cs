using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Calendar.ValueObjects
{
    public class TimeRange : ValueObject
    {
        public DateTime StartAt { get; private set; }
        public DateTime EndAt { get; private set; }

        protected TimeRange() { }

        public TimeRange(DateTime startAt, DateTime endAt)
        {
            if (startAt >= endAt)
                throw new BusinessException("Calendar:InvalidTimeRange");

            StartAt = startAt;
            EndAt = endAt;
        }

        public TimeSpan Duration => EndAt - StartAt;
        public int DurationMinutes => (int)Duration.TotalMinutes;
        public int DurationHours => (int)Duration.TotalHours;

        public bool OverlapsWith(TimeRange other)
        {
            return StartAt < other.EndAt && EndAt > other.StartAt;
        }

        public bool Contains(DateTime dateTime)
        {
            return dateTime >= StartAt && dateTime <= EndAt;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartAt;
            yield return EndAt;
        }
    }
}
