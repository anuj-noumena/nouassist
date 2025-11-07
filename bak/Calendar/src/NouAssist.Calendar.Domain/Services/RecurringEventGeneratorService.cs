using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Calendar.Aggregates;
using NouAssist.Calendar.Entities;
using NouAssist.Calendar.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.Calendar.Services
{
    public class RecurringEventGeneratorService : DomainService
    {
        public List<EventOccurrence> GenerateOccurrences(
            CalendarEvent recurringEvent,
            DateTime startDate,
            DateTime endDate,
            List<CalendarEventException> exceptions = null)
        {
            if (!recurringEvent.IsRecurring || recurringEvent.RecurrenceRule == null)
                return new List<EventOccurrence>();

            exceptions ??= new List<CalendarEventException>();
            var occurrences = new List<EventOccurrence>();

            var currentDate = startDate.Date;
            var generatedCount = 0;
            var maxOccurrences = recurringEvent.RecurrenceRule.Count ?? 1000;

            while (currentDate <= endDate && generatedCount < maxOccurrences)
            {
                if (recurringEvent.RecurrenceRule.Until.HasValue && 
                    currentDate > recurringEvent.RecurrenceRule.Until.Value)
                    break;

                if (ShouldGenerateOccurrence(currentDate, recurringEvent.RecurrenceRule))
                {
                    var exception = exceptions.FirstOrDefault(e => IsSameDate(e.InstanceDate, currentDate));
                    
                    if (exception == null || !exception.IsCancelled)
                    {
                        var occurrence = CreateOccurrence(recurringEvent, currentDate, exception);
                        occurrences.Add(occurrence);
                        generatedCount++;
                    }
                }

                currentDate = GetNextDate(currentDate, recurringEvent.RecurrenceRule);
            }

            return occurrences;
        }

        private bool ShouldGenerateOccurrence(DateTime date, RecurrenceRule rule)
        {
            switch (rule.Frequency.ToUpper())
            {
                case "DAILY":
                    return true;

                case "WEEKLY":
                    if (rule.ByDay != null && rule.ByDay.Any())
                        return rule.ByDay.Contains((int)date.DayOfWeek);
                    return true;

                case "MONTHLY":
                    if (rule.ByMonthDay != null && rule.ByMonthDay.Any())
                        return rule.ByMonthDay.Contains(date.Day);
                    return true;

                case "YEARLY":
                    if (rule.ByMonth != null && rule.ByMonth.Any())
                        return rule.ByMonth.Contains(date.Month);
                    return true;

                default:
                    return false;
            }
        }

        private DateTime GetNextDate(DateTime currentDate, RecurrenceRule rule)
        {
            switch (rule.Frequency.ToUpper())
            {
                case "DAILY":
                    return currentDate.AddDays(rule.Interval);
                case "WEEKLY":
                    return currentDate.AddDays(7 * rule.Interval);
                case "MONTHLY":
                    return currentDate.AddMonths(rule.Interval);
                case "YEARLY":
                    return currentDate.AddYears(rule.Interval);
                default:
                    return currentDate.AddDays(1);
            }
        }

        private EventOccurrence CreateOccurrence(
            CalendarEvent recurringEvent,
            DateTime instanceDate,
            CalendarEventException exception)
        {
            var startTime = recurringEvent.StartAt.TimeOfDay;
            var endTime = recurringEvent.EndAt.TimeOfDay;

            var occurrenceStart = instanceDate.Date.Add(startTime);
            var occurrenceEnd = instanceDate.Date.Add(endTime);

            if (exception != null && exception.ModifiedStart.HasValue)
            {
                occurrenceStart = exception.ModifiedStart.Value;
                occurrenceEnd = exception.ModifiedEnd.Value;
            }

            return new EventOccurrence
            {
                EventId = recurringEvent.Id,
                InstanceDate = instanceDate,
                Title = exception?.ModifiedTitle ?? recurringEvent.Title,
                Description = exception?.ModifiedDescription ?? recurringEvent.Description,
                Location = exception?.ModifiedLocation ?? recurringEvent.EventLocation,
                StartAt = occurrenceStart,
                EndAt = occurrenceEnd,
                IsCancelled = exception?.IsCancelled ?? false,
                IsModified = exception?.IsModified ?? false,
                ExceptionId = exception?.Id
            };
        }

        private bool IsSameDate(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year &&
                   date1.Month == date2.Month &&
                   date1.Day == date2.Day;
        }
    }

    public class EventOccurrence
    {
        public Guid EventId { get; set; }
        public DateTime InstanceDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsModified { get; set; }
        public Guid? ExceptionId { get; set; }
    }
}
