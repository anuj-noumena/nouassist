using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Calendar.Aggregates;
using NouAssist.Calendar.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Calendar.Services
{
    public class CalendarEventSchedulingService : DomainService
    {
        public SchedulingValidationResult ValidateEventScheduling(
            CalendarEvent newEvent,
            List<CalendarEvent> existingEvents,
            List<Timeblock> existingTimeblocks)
        {
            var result = new SchedulingValidationResult();

            // Check for overlaps with existing events
            if (!newEvent.AllowOverlap)
            {
                var overlappingEvents = existingEvents
                    .Where(e => !e.IsCancelled && !e.AllowOverlap)
                    .Where(e => EventsOverlap(newEvent, e))
                    .ToList();

                if (overlappingEvents.Any())
                {
                    result.AddError($"Event overlaps with {overlappingEvents.Count} existing event(s)");
                    result.OverlappingEventIds = overlappingEvents.Select(e => e.Id).ToList();
                }
            }

            // Check for overlaps with timeblocks
            var overlappingTimeblocks = existingTimeblocks
                .Where(tb => tb.Status != Enums.TimeblockStatus.Cancelled && !tb.AllowOverlap)
                .Where(tb => TimeblockOverlapsWithEvent(tb, newEvent))
                .ToList();

            if (overlappingTimeblocks.Any())
            {
                result.AddWarning($"Event overlaps with {overlappingTimeblocks.Count} timeblock(s)");
                result.OverlappingTimeblockIds = overlappingTimeblocks.Select(tb => tb.Id).ToList();
            }

            return result;
        }

        public List<TimeRange> FindAvailableSlots(
            DateTime startDate,
            DateTime endDate,
            int durationMinutes,
            List<CalendarEvent> existingEvents,
            List<Timeblock> existingTimeblocks)
        {
            var availableSlots = new List<TimeRange>();
            var workingStart = new DateTime(startDate.Year, startDate.Month, startDate.Day, 9, 0, 0);
            var workingEnd = new DateTime(startDate.Year, startDate.Month, startDate.Day, 17, 0, 0);

            var currentDate = startDate.Date;
            while (currentDate <= endDate.Date)
            {
                var dayStart = currentDate.AddHours(workingStart.Hour);
                var dayEnd = currentDate.AddHours(workingEnd.Hour);
                
                var busyRanges = GetBusyRanges(currentDate, existingEvents, existingTimeblocks);
                var freeRanges = CalculateFreeRanges(dayStart, dayEnd, busyRanges);

                foreach (var freeRange in freeRanges)
                {
                    if (freeRange.DurationMinutes >= durationMinutes)
                    {
                        availableSlots.Add(new TimeRange(
                            freeRange.StartAt,
                            freeRange.StartAt.AddMinutes(durationMinutes)));
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            return availableSlots;
        }

        private bool EventsOverlap(CalendarEvent event1, CalendarEvent event2)
        {
            return event1.StartAt < event2.EndAt && event1.EndAt > event2.StartAt;
        }

        private bool TimeblockOverlapsWithEvent(Timeblock timeblock, CalendarEvent calendarEvent)
        {
            return timeblock.StartAt < calendarEvent.EndAt && timeblock.EndAt > calendarEvent.StartAt;
        }

        private List<TimeRange> GetBusyRanges(
            DateTime date,
            List<CalendarEvent> events,
            List<Timeblock> timeblocks)
        {
            var ranges = new List<TimeRange>();

            ranges.AddRange(events
                .Where(e => !e.IsCancelled && e.StartAt.Date == date.Date)
                .Select(e => new TimeRange(e.StartAt, e.EndAt)));

            ranges.AddRange(timeblocks
                .Where(tb => tb.Status != Enums.TimeblockStatus.Cancelled && tb.StartAt.Date == date.Date)
                .Select(tb => new TimeRange(tb.StartAt, tb.EndAt)));

            return ranges.OrderBy(r => r.StartAt).ToList();
        }

        private List<TimeRange> CalculateFreeRanges(
            DateTime dayStart,
            DateTime dayEnd,
            List<TimeRange> busyRanges)
        {
            var freeRanges = new List<TimeRange>();
            var currentTime = dayStart;

            foreach (var busyRange in busyRanges.OrderBy(r => r.StartAt))
            {
                if (currentTime < busyRange.StartAt)
                {
                    freeRanges.Add(new TimeRange(currentTime, busyRange.StartAt));
                }
                currentTime = busyRange.EndAt > currentTime ? busyRange.EndAt : currentTime;
            }

            if (currentTime < dayEnd)
            {
                freeRanges.Add(new TimeRange(currentTime, dayEnd));
            }

            return freeRanges;
        }
    }

    public class SchedulingValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; } = new();
        public List<string> Warnings { get; } = new();
        public List<Guid> OverlappingEventIds { get; set; } = new();
        public List<Guid> OverlappingTimeblockIds { get; set; } = new();

        public void AddError(string error) => Errors.Add(error);
        public void AddWarning(string warning) => Warnings.Add(warning);
    }
}
