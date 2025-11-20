using System;
using System.Collections.Generic;
using NouAssist.Notification.Reminders.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.Reminders.ValueObjects
{
    public class ReminderSchedule : ValueObject
    {
        public DateTime BaseDateTime { get; init; }
        public int MinutesBeforeEvent { get; init; }
        public RecurrencePattern RecurrencePattern { get; init; }
        public int? RecurrenceInterval { get; init; }
        public DateTime? RecurrenceEndDate { get; init; }
        public List<DayOfWeek> RecurrenceDaysOfWeek { get; init; }

        public ReminderSchedule(
            DateTime baseDateTime,
            int minutesBeforeEvent = 0,
            RecurrencePattern recurrencePattern = RecurrencePattern.None,
            int? recurrenceInterval = null,
            DateTime? recurrenceEndDate = null,
            List<DayOfWeek> recurrenceDaysOfWeek = null)
        {
            BaseDateTime = baseDateTime;
            MinutesBeforeEvent = minutesBeforeEvent;
            RecurrencePattern = recurrencePattern;
            RecurrenceInterval = recurrenceInterval;
            RecurrenceEndDate = recurrenceEndDate;
            RecurrenceDaysOfWeek = recurrenceDaysOfWeek ?? new List<DayOfWeek>();
        }

        public DateTime CalculateReminderTime()
        {
            return BaseDateTime.AddMinutes(-MinutesBeforeEvent);
        }

        public DateTime? CalculateNextOccurrence(DateTime afterDate)
        {
            if (RecurrencePattern == RecurrencePattern.None)
            {
                return null;
            }

            if (RecurrenceEndDate.HasValue && afterDate >= RecurrenceEndDate.Value)
            {
                return null;
            }

            var interval = RecurrenceInterval ?? 1;
            var nextDate = afterDate;

            switch (RecurrencePattern)
            {
                case RecurrencePattern.Daily:
                    nextDate = afterDate.AddDays(interval);
                    break;

                case RecurrencePattern.Weekly:
                    nextDate = CalculateNextWeeklyOccurrence(afterDate, interval);
                    break;

                case RecurrencePattern.Monthly:
                    nextDate = afterDate.AddMonths(interval);
                    break;

                case RecurrencePattern.Yearly:
                    nextDate = afterDate.AddYears(interval);
                    break;
            }

            if (RecurrenceEndDate.HasValue && nextDate > RecurrenceEndDate.Value)
            {
                return null;
            }

            return nextDate;
        }

        private DateTime CalculateNextWeeklyOccurrence(DateTime afterDate, int interval)
        {
            if (RecurrenceDaysOfWeek == null || RecurrenceDaysOfWeek.Count == 0)
            {
                return afterDate.AddDays(7 * interval);
            }

            var currentDate = afterDate.AddDays(1);
            var weeksChecked = 0;

            while (weeksChecked < 52) // Safety limit
            {
                if (RecurrenceDaysOfWeek.Contains(currentDate.DayOfWeek))
                {
                    return currentDate;
                }

                currentDate = currentDate.AddDays(1);
                
                if (currentDate.DayOfWeek == afterDate.DayOfWeek)
                {
                    weeksChecked++;
                }
            }

            return afterDate.AddDays(7 * interval);
        }

        public bool IsRecurring => RecurrencePattern != RecurrencePattern.None;

        public bool HasEndDate => RecurrenceEndDate.HasValue;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BaseDateTime;
            yield return MinutesBeforeEvent;
            yield return RecurrencePattern;
            yield return RecurrenceInterval;
            yield return RecurrenceEndDate;
            yield return string.Join(",", RecurrenceDaysOfWeek);
        }
    }
}
