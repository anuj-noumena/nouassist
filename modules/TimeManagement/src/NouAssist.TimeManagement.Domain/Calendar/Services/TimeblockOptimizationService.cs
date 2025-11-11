using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.TimeManagement.Calendar.Services
{
    /// <summary>
    /// Domain service for optimizing timeblock scheduling and allocation
    /// </summary>
    public class TimeblockOptimizationService : DomainService
    {
        /// <summary>
        /// Suggests optimal timeblocks based on available time slots and task priorities
        /// </summary>
        public List<TimeblockSuggestion> SuggestOptimalTimeblocks(
            List<CalendarEvent> existingEvents,
            List<Timeblock> existingTimeblocks,
            List<TaskEstimate> tasksToSchedule,
            DateTime startDate,
            DateTime endDate,
            WorkingHoursPreference workingHours)
        {
            var suggestions = new List<TimeblockSuggestion>();
            var currentDate = startDate.Date;

            while (currentDate <= endDate.Date)
            {
                var dayStart = currentDate.Add(workingHours.StartTime);
                var dayEnd = currentDate.Add(workingHours.EndTime);

                var availableSlots = CalculateAvailableSlots(
                    dayStart,
                    dayEnd,
                    existingEvents.Where(e => e.StartAt.Date == currentDate.Date).ToList(),
                    existingTimeblocks.Where(tb => tb.StartAt.Date == currentDate.Date).ToList());

                foreach (var task in tasksToSchedule.OrderByDescending(t => t.Priority))
                {
                    var bestSlot = FindBestSlotForTask(availableSlots, task, workingHours);
                    if (bestSlot != null)
                    {
                        suggestions.Add(new TimeblockSuggestion
                        {
                            TaskId = task.TaskId,
                            TaskTitle = task.Title,
                            SuggestedStart = bestSlot.Start,
                            SuggestedEnd = bestSlot.End,
                            Reason = $"Optimal focus time based on {task.PreferredTimeOfDay} preference",
                            Score = CalculateSlotScore(bestSlot, task, workingHours)
                        });

                        // Remove this slot from available slots
                        availableSlots.Remove(bestSlot);
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            return suggestions.OrderByDescending(s => s.Score).ToList();
        }

        /// <summary>
        /// Analyzes timeblock completion patterns to improve future estimates
        /// </summary>
        public TimeblockAnalysis AnalyzeTimeblockPatterns(List<Timeblock> completedTimeblocks)
        {
            var analysis = new TimeblockAnalysis();

            var byType = completedTimeblocks
                .Where(tb => tb.Status == TimeblockStatus.Completed)
                .GroupBy(tb => tb.BlockType)
                .ToList();

            foreach (var group in byType)
            {
                var avgDuration = group.Average(tb => tb.DurationInMinutes);
                var totalCount = group.Count();
                var successRate = group.Count(tb => !string.IsNullOrEmpty(tb.CompletionNotes)) / (double)totalCount;

                analysis.TypeStatistics[group.Key] = new TypeStats
                {
                    AverageDuration = (int)avgDuration,
                    TotalCompleted = totalCount,
                    SuccessRate = successRate * 100
                };
            }

            // Calculate best times for focus work
            var focusWorkBlocks = completedTimeblocks
                .Where(tb => tb.BlockType == TimeblockType.FocusWork && tb.Status == TimeblockStatus.Completed)
                .ToList();

            if (focusWorkBlocks.Any())
            {
                var hourlySuccess = focusWorkBlocks
                    .GroupBy(tb => tb.StartAt.Hour)
                    .Select(g => new
                    {
                        Hour = g.Key,
                        SuccessRate = g.Count(tb => !string.IsNullOrEmpty(tb.CompletionNotes)) / (double)g.Count()
                    })
                    .OrderByDescending(x => x.SuccessRate)
                    .Take(3)
                    .ToList();

                analysis.BestFocusHours = hourlySuccess.Select(h => h.Hour).ToList();
            }

            return analysis;
        }

        private List<TimeSlot> CalculateAvailableSlots(
            DateTime dayStart,
            DateTime dayEnd,
            List<CalendarEvent> events,
            List<Timeblock> timeblocks)
        {
            var availableSlots = new List<TimeSlot>();
            var busyPeriods = new List<TimeSlot>();

            // Add events as busy periods
            busyPeriods.AddRange(events
                .Where(e => !e.IsCancelled)
                .Select(e => new TimeSlot { Start = e.StartAt, End = e.EndAt }));

            // Add timeblocks as busy periods
            busyPeriods.AddRange(timeblocks
                .Where(tb => tb.Status != TimeblockStatus.Cancelled)
                .Select(tb => new TimeSlot { Start = tb.StartAt, End = tb.EndAt }));

            busyPeriods = busyPeriods.OrderBy(p => p.Start).ToList();

            var currentTime = dayStart;
            foreach (var busyPeriod in busyPeriods)
            {
                if (currentTime < busyPeriod.Start)
                {
                    availableSlots.Add(new TimeSlot
                    {
                        Start = currentTime,
                        End = busyPeriod.Start
                    });
                }
                currentTime = busyPeriod.End > currentTime ? busyPeriod.End : currentTime;
            }

            if (currentTime < dayEnd)
            {
                availableSlots.Add(new TimeSlot
                {
                    Start = currentTime,
                    End = dayEnd
                });
            }

            return availableSlots.Where(s => s.DurationMinutes >= 15).ToList();
        }

        private TimeSlot FindBestSlotForTask(
            List<TimeSlot> availableSlots,
            TaskEstimate task,
            WorkingHoursPreference workingHours)
        {
            return availableSlots
                .Where(slot => slot.DurationMinutes >= task.EstimatedMinutes)
                .OrderByDescending(slot => CalculateSlotScore(slot, task, workingHours))
                .FirstOrDefault();
        }

        private double CalculateSlotScore(TimeSlot slot, TaskEstimate task, WorkingHoursPreference workingHours)
        {
            double score = 100.0;

            // Prefer slots that match task's preferred time of day
            var slotHour = slot.Start.Hour;
            var preferredHourStart = task.PreferredTimeOfDay == "morning" ? 8 : 
                                     task.PreferredTimeOfDay == "afternoon" ? 13 : 16;
            var hourDifference = Math.Abs(slotHour - preferredHourStart);
            score -= hourDifference * 5;

            // Prefer slots that match the task duration closely
            var durationMatch = Math.Abs(slot.DurationMinutes - task.EstimatedMinutes);
            score -= durationMatch * 0.1;

            // Boost score for slots during peak productivity hours
            if (workingHours.PeakHours != null && workingHours.PeakHours.Contains(slotHour))
                score += 20;

            return Math.Max(0, score);
        }
    }

    public class TimeblockSuggestion
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; }
        public DateTime SuggestedStart { get; set; }
        public DateTime SuggestedEnd { get; set; }
        public string Reason { get; set; }
        public double Score { get; set; }
    }

    public class TimeblockAnalysis
    {
        public Dictionary<TimeblockType, TypeStats> TypeStatistics { get; set; } = new();
        public List<int> BestFocusHours { get; set; } = new();
        public Dictionary<string, object> Insights { get; set; } = new();
    }

    public class TypeStats
    {
        public int AverageDuration { get; set; }
        public int TotalCompleted { get; set; }
        public double SuccessRate { get; set; }
    }

    public class TimeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int DurationMinutes => (int)(End - Start).TotalMinutes;
    }

    public class TaskEstimate
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public int EstimatedMinutes { get; set; }
        public int Priority { get; set; }
        public string PreferredTimeOfDay { get; set; } // "morning", "afternoon", "evening"
    }

    public class WorkingHoursPreference
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<int> PeakHours { get; set; } = new();
    }
}
