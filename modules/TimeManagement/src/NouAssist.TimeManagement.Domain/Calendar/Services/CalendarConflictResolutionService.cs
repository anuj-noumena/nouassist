using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.TimeManagement.Calendar.Aggregates;
using Volo.Abp.Domain.Services;

namespace NouAssist.TimeManagement.Calendar.Services
{
    /// <summary>
    /// Domain service for detecting and resolving calendar conflicts
    /// </summary>
    public class CalendarConflictResolutionService : DomainService
    {
        public ConflictDetectionResult DetectConflicts(
            CalendarEvent proposedEvent,
            List<CalendarEvent> existingEvents,
            List<Timeblock> existingTimeblocks)
        {
            var result = new ConflictDetectionResult
            {
                ProposedEvent = proposedEvent
            };

            // Find hard conflicts (overlapping non-flexible events)
            var hardConflicts = existingEvents
                .Where(e => !e.IsCancelled && !e.AllowOverlap && EventsOverlap(e, proposedEvent))
                .Select(e => new EventConflict
                {
                    ConflictingEventId = e.Id,
                    ConflictingEventTitle = e.Title,
                    ConflictType = ConflictType.Hard,
                    ConflictStart = Max(e.StartAt, proposedEvent.StartAt),
                    ConflictEnd = Min(e.EndAt, proposedEvent.EndAt),
                    Severity = ConflictSeverity.High
                })
                .ToList();

            result.Conflicts.AddRange(hardConflicts);

            // Find soft conflicts (overlapping with flexible timeblocks)
            var softConflicts = existingTimeblocks
                .Where(tb => tb.IsFlexible && tb.Status != Enums.TimeblockStatus.Cancelled && 
                            TimeblockOverlapsWithEvent(tb, proposedEvent))
                .Select(tb => new EventConflict
                {
                    ConflictingEventId = tb.Id,
                    ConflictingEventTitle = tb.Title ?? "Timeblock",
                    ConflictType = ConflictType.Soft,
                    ConflictStart = Max(tb.StartAt, proposedEvent.StartAt),
                    ConflictEnd = Min(tb.EndAt, proposedEvent.EndAt),
                    Severity = ConflictSeverity.Medium
                })
                .ToList();

            result.Conflicts.AddRange(softConflicts);

            return result;
        }

        public List<ConflictResolutionOption> SuggestResolutions(ConflictDetectionResult conflictResult)
        {
            var options = new List<ConflictResolutionOption>();

            if (!conflictResult.HasConflicts)
            {
                return options;
            }

            var proposedEvent = conflictResult.ProposedEvent;

            // Option 1: Reschedule proposed event to nearest available slot
            options.Add(new ConflictResolutionOption
            {
                Strategy = ResolutionStrategy.RescheduleProposed,
                Description = "Reschedule to next available slot",
                Priority = 1,
                Impact = "Proposed event moved to different time"
            });

            // Option 2: Allow overlap if conflicts are only soft
            if (conflictResult.Conflicts.All(c => c.ConflictType == ConflictType.Soft))
            {
                options.Add(new ConflictResolutionOption
                {
                    Strategy = ResolutionStrategy.AllowOverlap,
                    Description = "Allow overlap with flexible timeblocks",
                    Priority = 2,
                    Impact = "Flexible timeblocks may need to be rescheduled"
                });
            }

            // Option 3: Split the proposed event
            if (proposedEvent.DurationInMinutes >= 60)
            {
                options.Add(new ConflictResolutionOption
                {
                    Strategy = ResolutionStrategy.SplitEvent,
                    Description = "Split into multiple shorter sessions",
                    Priority = 3,
                    Impact = "Event split into smaller time blocks"
                });
            }

            return options.OrderBy(o => o.Priority).ToList();
        }

        public ConflictResolutionResult ApplyResolution(
            ConflictDetectionResult conflictResult,
            ResolutionStrategy strategy,
            List<CalendarEvent> existingEvents,
            List<Timeblock> existingTimeblocks)
        {
            var result = new ConflictResolutionResult
            {
                Strategy = strategy,
                Success = false
            };

            switch (strategy)
            {
                case ResolutionStrategy.RescheduleProposed:
                    result = RescheduleToAvailableSlot(conflictResult.ProposedEvent, existingEvents, existingTimeblocks);
                    break;

                case ResolutionStrategy.AllowOverlap:
                    conflictResult.ProposedEvent.AllowOverlap = true;
                    result.Success = true;
                    result.Message = "Overlap allowed for this event";
                    break;

                case ResolutionStrategy.SplitEvent:
                    result = SplitEventIntoSessions(conflictResult.ProposedEvent, existingEvents);
                    break;
            }

            return result;
        }

        private ConflictResolutionResult RescheduleToAvailableSlot(
            CalendarEvent proposedEvent,
            List<CalendarEvent> existingEvents,
            List<Timeblock> existingTimeblocks)
        {
            var result = new ConflictResolutionResult
            {
                Strategy = ResolutionStrategy.RescheduleProposed
            };

            var searchStart = proposedEvent.StartAt;
            var maxDaysToSearch = 7;
            var durationMinutes = proposedEvent.DurationInMinutes;

            for (int day = 0; day < maxDaysToSearch; day++)
            {
                var targetDate = searchStart.AddDays(day).Date;
                var dayStart = targetDate.AddHours(8); // 8 AM
                var dayEnd = targetDate.AddHours(18);  // 6 PM

                var availableSlots = FindAvailableSlotsForDay(
                    dayStart, 
                    dayEnd, 
                    durationMinutes,
                    existingEvents.Where(e => e.StartAt.Date == targetDate).ToList(),
                    existingTimeblocks.Where(tb => tb.StartAt.Date == targetDate).ToList());

                if (availableSlots.Any())
                {
                    var bestSlot = availableSlots.First();
                    proposedEvent.Reschedule(bestSlot.Start, bestSlot.End);
                    
                    result.Success = true;
                    result.Message = $"Event rescheduled to {bestSlot.Start:g}";
                    result.NewStartTime = bestSlot.Start;
                    result.NewEndTime = bestSlot.End;
                    return result;
                }
            }

            result.Success = false;
            result.Message = "No available slots found within 7 days";
            return result;
        }

        private ConflictResolutionResult SplitEventIntoSessions(
            CalendarEvent proposedEvent,
            List<CalendarEvent> existingEvents)
        {
            var result = new ConflictResolutionResult
            {
                Strategy = ResolutionStrategy.SplitEvent,
                Success = false,
                Message = "Event splitting requires manual intervention"
            };

            // This would typically require user input for how to split
            // For now, we just indicate it's possible
            return result;
        }

        private List<TimeSlot> FindAvailableSlotsForDay(
            DateTime dayStart,
            DateTime dayEnd,
            int requiredMinutes,
            List<CalendarEvent> events,
            List<Timeblock> timeblocks)
        {
            var busyPeriods = new List<TimeSlot>();

            busyPeriods.AddRange(events
                .Where(e => !e.IsCancelled && !e.AllowOverlap)
                .Select(e => new TimeSlot { Start = e.StartAt, End = e.EndAt }));

            busyPeriods.AddRange(timeblocks
                .Where(tb => !tb.AllowOverlap && tb.Status != Enums.TimeblockStatus.Cancelled)
                .Select(tb => new TimeSlot { Start = tb.StartAt, End = tb.EndAt }));

            busyPeriods = busyPeriods.OrderBy(p => p.Start).ToList();

            var availableSlots = new List<TimeSlot>();
            var currentTime = dayStart;

            foreach (var busy in busyPeriods)
            {
                if (currentTime < busy.Start)
                {
                    var gapMinutes = (int)(busy.Start - currentTime).TotalMinutes;
                    if (gapMinutes >= requiredMinutes)
                    {
                        availableSlots.Add(new TimeSlot
                        {
                            Start = currentTime,
                            End = currentTime.AddMinutes(requiredMinutes)
                        });
                    }
                }
                currentTime = busy.End > currentTime ? busy.End : currentTime;
            }

            if (currentTime < dayEnd)
            {
                var remainingMinutes = (int)(dayEnd - currentTime).TotalMinutes;
                if (remainingMinutes >= requiredMinutes)
                {
                    availableSlots.Add(new TimeSlot
                    {
                        Start = currentTime,
                        End = currentTime.AddMinutes(requiredMinutes)
                    });
                }
            }

            return availableSlots;
        }

        private bool EventsOverlap(CalendarEvent e1, CalendarEvent e2)
        {
            return e1.StartAt < e2.EndAt && e1.EndAt > e2.StartAt;
        }

        private bool TimeblockOverlapsWithEvent(Timeblock tb, CalendarEvent e)
        {
            return tb.StartAt < e.EndAt && tb.EndAt > e.StartAt;
        }

        private DateTime Max(DateTime d1, DateTime d2) => d1 > d2 ? d1 : d2;
        private DateTime Min(DateTime d1, DateTime d2) => d1 < d2 ? d1 : d2;
    }

    public class ConflictDetectionResult
    {
        public CalendarEvent ProposedEvent { get; set; }
        public List<EventConflict> Conflicts { get; set; } = new();
        public bool HasConflicts => Conflicts.Any();
        public bool HasHardConflicts => Conflicts.Any(c => c.ConflictType == ConflictType.Hard);
    }

    public class EventConflict
    {
        public Guid ConflictingEventId { get; set; }
        public string ConflictingEventTitle { get; set; }
        public ConflictType ConflictType { get; set; }
        public DateTime ConflictStart { get; set; }
        public DateTime ConflictEnd { get; set; }
        public ConflictSeverity Severity { get; set; }
    }

    public class ConflictResolutionOption
    {
        public ResolutionStrategy Strategy { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Impact { get; set; }
    }

    public class ConflictResolutionResult
    {
        public ResolutionStrategy Strategy { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime? NewStartTime { get; set; }
        public DateTime? NewEndTime { get; set; }
    }

    public enum ConflictType
    {
        Hard,
        Soft
    }

    public enum ConflictSeverity
    {
        Low,
        Medium,
        High
    }

    public enum ResolutionStrategy
    {
        RescheduleProposed,
        RescheduleConflicting,
        AllowOverlap,
        SplitEvent,
        CancelConflicting
    }
}
