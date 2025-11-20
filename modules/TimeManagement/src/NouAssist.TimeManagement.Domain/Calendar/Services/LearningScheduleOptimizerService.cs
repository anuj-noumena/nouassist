using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.TimeManagement.Calendar.Data;
using Volo.Abp.Domain.Services;

namespace NouAssist.TimeManagement.Calendar.Services
{
    /// <summary>
    /// Service for optimizing learning schedule based on historical patterns
    /// </summary>
    public class LearningScheduleOptimizerService : DomainService
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IUserAvailabilityRepository _availabilityRepository;
        private readonly ITimeblockRepository _timeblockRepository;

        public LearningScheduleOptimizerService(
            ITimeEntryRepository timeEntryRepository,
            IUserAvailabilityRepository availabilityRepository,
            ITimeblockRepository timeblockRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _availabilityRepository = availabilityRepository;
            _timeblockRepository = timeblockRepository;
        }

        public async Task<List<TimeSlotRecommendation>> RecommendLearningTimesAsync(
            Guid userId,
            int durationMinutes,
            DayOfWeek[] preferredDays = null,
            int recommendationCount = 5)
        {
            // Analyze historical learning time entries
            var historicalData = await GetLearningHistoryAsync(userId);

            // Find productivity patterns
            var productivityPatterns = AnalyzeProductivityPatterns(historicalData);

            // Check calendar availability for next 30 days
            var availableSlots = await GetAvailableSlotsAsync(userId, DateTime.Today, DateTime.Today.AddDays(30));

            // Filter by preferred days if specified
            if (preferredDays != null && preferredDays.Length > 0)
            {
                availableSlots = availableSlots
                    .Where(slot => preferredDays.Contains(slot.Date.DayOfWeek))
                    .ToList();
            }

            // Score each slot based on productivity patterns
            var scoredSlots = ScoreSlotsByProductivity(availableSlots, productivityPatterns, durationMinutes);

            // Return top recommendations
            return scoredSlots
                .OrderByDescending(s => s.Score)
                .Take(recommendationCount)
                .ToList();
        }

        private async Task<List<Aggregates.TimeEntry>> GetLearningHistoryAsync(Guid userId)
        {
            var thirtyDaysAgo = DateTime.Today.AddDays(-30);
            var allEntries = await _timeEntryRepository.GetEntriesInRangeAsync(
                userId, thirtyDaysAgo, DateTime.Today);

            return allEntries
                .Where(te => te.Tags.Any(t => t == "Learning"))
                .OrderBy(te => te.StartedAt)
                .ToList();
        }

        private ProductivityPatterns AnalyzeProductivityPatterns(List<Aggregates.TimeEntry> history)
        {
            if (!history.Any())
            {
                // Return default patterns if no history
                return new ProductivityPatterns
                {
                    BestHours = new List<int> { 9, 10, 11 }, // Morning hours by default
                    BestDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                    AverageSessionLength = 60
                };
            }

            // Analyze hour of day productivity
            var hourlyProductivity = history
                .GroupBy(te => te.StartedAt.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    AvgDuration = g.Average(te => te.DurationMinutes ?? 0),
                    SessionCount = g.Count()
                })
                .OrderByDescending(x => x.AvgDuration)
                .Take(3)
                .Select(x => x.Hour)
                .ToList();

            // Analyze day of week productivity
            var dayProductivity = history
                .GroupBy(te => te.StartedAt.DayOfWeek)
                .Select(g => new
                {
                    Day = g.Key,
                    AvgDuration = g.Average(te => te.DurationMinutes ?? 0),
                    SessionCount = g.Count()
                })
                .OrderByDescending(x => x.SessionCount)
                .Take(3)
                .Select(x => x.Day)
                .ToList();

            return new ProductivityPatterns
            {
                BestHours = hourlyProductivity.Any() ? hourlyProductivity : new List<int> { 9, 10, 11 },
                BestDays = dayProductivity.Any() ? dayProductivity : new List<DayOfWeek>
                {
                    DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday
                },
                AverageSessionLength = (int)history.Average(te => te.DurationMinutes ?? 60)
            };
        }

        private async Task<List<AvailableSlot>> GetAvailableSlotsAsync(
            Guid userId,
            DateTime startDate,
            DateTime endDate)
        {
            var slots = new List<AvailableSlot>();

            // Get existing timeblocks to avoid conflicts
            var existingTimeblocks = await _timeblockRepository.GetTimeblocksInRangeAsync(
                userId, startDate, endDate);

            // Generate potential slots for each day
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check typical working hours (8 AM to 8 PM)
                for (int hour = 8; hour < 20; hour++)
                {
                    var slotStart = date.Date.AddHours(hour);
                    var slotEnd = slotStart.AddHours(1);

                    // Check if this slot conflicts with existing timeblocks
                    var hasConflict = existingTimeblocks.Any(tb =>
                        (slotStart >= tb.StartAt && slotStart < tb.EndAt) ||
                        (slotEnd > tb.StartAt && slotEnd <= tb.EndAt));

                    if (!hasConflict)
                    {
                        slots.Add(new AvailableSlot
                        {
                            Date = date,
                            Hour = hour,
                            StartTime = slotStart,
                            EndTime = slotEnd
                        });
                    }
                }
            }

            return slots;
        }

        private List<TimeSlotRecommendation> ScoreSlotsByProductivity(
            List<AvailableSlot> slots,
            ProductivityPatterns patterns,
            int durationMinutes)
        {
            return slots.Select(slot =>
            {
                var score = 0;

                // Score based on hour of day (max 50 points)
                if (patterns.BestHours.Contains(slot.Hour))
                {
                    var index = patterns.BestHours.IndexOf(slot.Hour);
                    score += 50 - (index * 10); // First best hour gets 50, second 40, third 30
                }

                // Score based on day of week (max 30 points)
                if (patterns.BestDays.Contains(slot.Date.DayOfWeek))
                {
                    var index = patterns.BestDays.IndexOf(slot.Date.DayOfWeek);
                    score += 30 - (index * 10);
                }

                // Score based on proximity to average session length (max 20 points)
                var durationDiff = Math.Abs(patterns.AverageSessionLength - durationMinutes);
                if (durationDiff < 15)
                {
                    score += 20;
                }
                else if (durationDiff < 30)
                {
                    score += 10;
                }

                return new TimeSlotRecommendation
                {
                    Date = slot.Date,
                    StartTime = slot.StartTime,
                    EndTime = slot.StartTime.AddMinutes(durationMinutes),
                    Score = score,
                    Reason = GenerateRecommendationReason(slot, patterns, score)
                };
            }).ToList();
        }

        private string GenerateRecommendationReason(
            AvailableSlot slot,
            ProductivityPatterns patterns,
            int score)
        {
            var reasons = new List<string>();

            if (patterns.BestHours.Contains(slot.Hour))
            {
                reasons.Add($"Peak productivity time ({slot.Hour}:00)");
            }

            if (patterns.BestDays.Contains(slot.Date.DayOfWeek))
            {
                reasons.Add($"Historically productive on {slot.Date.DayOfWeek}s");
            }

            if (reasons.Any())
            {
                return string.Join(". ", reasons);
            }

            return "Available time slot";
        }
    }

    public class TimeSlotRecommendation
    {
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Score { get; set; }
        public string Reason { get; set; }
    }

    public class ProductivityPatterns
    {
        public List<int> BestHours { get; set; }
        public List<DayOfWeek> BestDays { get; set; }
        public int AverageSessionLength { get; set; }
    }

    public class AvailableSlot
    {
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
