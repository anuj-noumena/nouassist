using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Calendar.ValueObjects
{
    public class RecurrenceRule : ValueObject
    {
        public string Frequency { get; private set; } // DAILY, WEEKLY, MONTHLY, YEARLY
        public int Interval { get; private set; }
        public DateTime? Until { get; private set; }
        public int? Count { get; private set; }
        public List<int> ByDay { get; private set; } // Days of week (0-6)
        public List<int> ByMonthDay { get; private set; }
        public List<int> ByMonth { get; private set; }

        protected RecurrenceRule() { }

        public RecurrenceRule(
            string frequency,
            int interval = 1,
            DateTime? until = null,
            int? count = null,
            List<int> byDay = null,
            List<int> byMonthDay = null,
            List<int> byMonth = null)
        {
            Frequency = Check.NotNullOrWhiteSpace(frequency, nameof(frequency));
            
            if (interval < 1)
                throw new BusinessException("Calendar:InvalidRecurrenceRule")
                    .WithData("reason", "Interval must be at least 1");

            if (until.HasValue && count.HasValue)
                throw new BusinessException("Calendar:InvalidRecurrenceRule")
                    .WithData("reason", "Cannot specify both Until and Count");

            Interval = interval;
            Until = until;
            Count = count;
            ByDay = byDay ?? new List<int>();
            ByMonthDay = byMonthDay ?? new List<int>();
            ByMonth = byMonth ?? new List<int>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Frequency;
            yield return Interval;
            yield return Until;
            yield return Count;
            yield return string.Join(",", ByDay);
            yield return string.Join(",", ByMonthDay);
            yield return string.Join(",", ByMonth);
        }
    }
}
