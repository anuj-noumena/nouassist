using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public enum RecurrenceFrequency
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public class RecurrenceRule : ValueObject
    {
        public RecurrenceFrequency Frequency { get; private set; }
        public int Interval { get; private set; }
        public DateTime? EndDate { get; private set; }
        public int? Count { get; private set; }
        public List<DayOfWeek> DaysOfWeek { get; private set; }

        private RecurrenceRule() { }

        public RecurrenceRule(
            RecurrenceFrequency frequency,
            int interval = 1,
            DateTime? endDate = null,
            int? count = null,
            List<DayOfWeek> daysOfWeek = null)
        {
            if (interval < 1)
            {
                throw new BusinessException("Tasks:InvalidRecurrenceInterval");
            }

            if (endDate.HasValue && count.HasValue)
            {
                throw new BusinessException("Tasks:CannotHaveBothEndDateAndCount");
            }

            Frequency = frequency;
            Interval = interval;
            EndDate = endDate;
            Count = count;
            DaysOfWeek = daysOfWeek ?? new List<DayOfWeek>();
        }

        public DateTime CalculateNextOccurrence(DateTime currentDate)
        {
            return Frequency switch
            {
                RecurrenceFrequency.Daily => currentDate.AddDays(Interval),
                RecurrenceFrequency.Weekly => currentDate.AddDays(7 * Interval),
                RecurrenceFrequency.Monthly => currentDate.AddMonths(Interval),
                RecurrenceFrequency.Yearly => currentDate.AddYears(Interval),
                _ => throw new InvalidOperationException("Invalid frequency")
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Frequency;
            yield return Interval;
            yield return EndDate;
            yield return Count;
            foreach (var day in DaysOfWeek)
            {
                yield return day;
            }
        }
    }
}
