using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.SharedKernel.ValueObjects
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
        public DateTime? Until { get; private set; }
        public int? Count { get; private set; }
        public List<int> ByWeekDay { get; private set; }
        public List<int> ByMonthDay { get; private set; }
        public string? Description { get; private set; }

        private RecurrenceRule() 
        {
            ByWeekDay = new List<int>();
            ByMonthDay = new List<int>();
        }

        public RecurrenceRule(
            RecurrenceFrequency frequency,
            int interval = 1,
            DateTime? until = null,
            int? count = null,
            List<int>? byWeekDay = null,
            List<int>? byMonthDay = null,
            string? description = null)
        {
            if (interval <= 0)
            {
                throw new BusinessException("Core:InvalidRecurrenceInterval", 
                    "Interval must be greater than 0.");
            }

            if (until.HasValue && count.HasValue)
            {
                throw new BusinessException("Core:InvalidRecurrenceRule", 
                    "Cannot specify both 'until' and 'count'.");
            }

            Frequency = frequency;
            Interval = interval;
            Until = until;
            Count = count;
            ByWeekDay = byWeekDay ?? new List<int>();
            ByMonthDay = byMonthDay ?? new List<int>();
            Description = description ?? GenerateDescription();
        }

        public static RecurrenceRule Daily(int interval = 1) => 
            new(RecurrenceFrequency.Daily, interval);

        public static RecurrenceRule Weekly(int interval = 1, List<int>? weekDays = null) => 
            new(RecurrenceFrequency.Weekly, interval, byWeekDay: weekDays);

        public static RecurrenceRule Monthly(int interval = 1, List<int>? monthDays = null) => 
            new(RecurrenceFrequency.Monthly, interval, byMonthDay: monthDays);

        public static RecurrenceRule Yearly(int interval = 1) => 
            new(RecurrenceFrequency.Yearly, interval);

        public DateTime? GetNextOccurrence(DateTime after)
        {
            var next = after;
            
            switch (Frequency)
            {
                case RecurrenceFrequency.Daily:
                    next = after.AddDays(Interval);
                    break;
                case RecurrenceFrequency.Weekly:
                    next = after.AddDays(7 * Interval);
                    break;
                case RecurrenceFrequency.Monthly:
                    next = after.AddMonths(Interval);
                    break;
                case RecurrenceFrequency.Yearly:
                    next = after.AddYears(Interval);
                    break;
            }

            if (Until.HasValue && next > Until.Value)
                return null;

            return next;
        }

        private string GenerateDescription()
        {
            var intervalText = Interval == 1 ? "" : $"every {Interval} ";
            var frequencyText = Frequency.ToString().ToLower();
            if (Interval > 1) frequencyText += "s";
            
            return $"Repeats {intervalText}{frequencyText}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Frequency;
            yield return Interval;
            yield return Until ?? DateTime.MinValue;
            yield return Count ?? 0;
            yield return string.Join(",", ByWeekDay);
            yield return string.Join(",", ByMonthDay);
        }
    }
}
