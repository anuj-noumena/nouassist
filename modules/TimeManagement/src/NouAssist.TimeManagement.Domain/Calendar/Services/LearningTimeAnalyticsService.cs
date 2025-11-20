using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.TimeManagement.Calendar.Data;
using Volo.Abp.Domain.Services;

namespace NouAssist.TimeManagement.Calendar.Services
{
    /// <summary>
    /// Service for analyzing learning-related time tracking data
    /// </summary>
    public class LearningTimeAnalyticsService : DomainService
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITimeblockRepository _timeblockRepository;

        public LearningTimeAnalyticsService(
            ITimeEntryRepository timeEntryRepository,
            ITimeblockRepository timeblockRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _timeblockRepository = timeblockRepository;
        }

        public async Task<LearningTimeReport> GetLearningTimeReportAsync(
            Guid userId,
            DateTime fromDate,
            DateTime toDate)
        {
            var timeEntries = await _timeEntryRepository.GetByOwnerAndDateRangeAsync(
                userId, fromDate, toDate);

            var learningEntries = timeEntries
                .Where(te => te.Tags.Any(t => t == "Learning"))
                .ToList();

            if (!learningEntries.Any())
            {
                return new LearningTimeReport
                {
                    UserId = userId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    TotalMinutes = 0
                };
            }

            var totalMinutes = learningEntries.Sum(te => te.DurationMinutes ?? 0);
            var byPhase = learningEntries
                .SelectMany(te => te.Tags.Where(t => t != "Learning"))
                .GroupBy(phase => phase)
                .ToDictionary(g => g.Key, g => learningEntries
                    .Where(te => te.Tags.Contains(g.Key))
                    .Sum(te => te.DurationMinutes ?? 0));

            var avgSessionLength = learningEntries.Average(te => te.DurationMinutes ?? 0);
            var mostProductiveHour = CalculateMostProductiveHour(learningEntries);

            return new LearningTimeReport
            {
                UserId = userId,
                FromDate = fromDate,
                ToDate = toDate,
                TotalMinutes = totalMinutes,
                ByPhase = byPhase,
                AverageSessionLengthMinutes = (int)avgSessionLength,
                MostProductiveTimeOfDay = mostProductiveHour,
                SessionCount = learningEntries.Count,
                LongestSessionMinutes = learningEntries.Max(te => te.DurationMinutes ?? 0),
                ShortestSessionMinutes = learningEntries.Min(te => te.DurationMinutes ?? 0)
            };
        }

        private string CalculateMostProductiveHour(List<Aggregates.TimeEntry> entries)
        {
            var hourlyTotals = entries
                .GroupBy(te => te.StartedAt.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    TotalMinutes = g.Sum(te => te.DurationMinutes ?? 0)
                })
                .OrderByDescending(x => x.TotalMinutes)
                .FirstOrDefault();

            if (hourlyTotals == null)
            {
                return "N/A";
            }

            var startHour = hourlyTotals.Hour;
            var endHour = (startHour + 1) % 24;
            return $"{startHour:D2}:00-{endHour:D2}:00";
        }
    }

    public class LearningTimeReport
    {
        public Guid UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalMinutes { get; set; }
        public Dictionary<string, int> ByPhase { get; set; } = new();
        public int AverageSessionLengthMinutes { get; set; }
        public string MostProductiveTimeOfDay { get; set; }
        public int SessionCount { get; set; }
        public int LongestSessionMinutes { get; set; }
        public int ShortestSessionMinutes { get; set; }
    }
}
