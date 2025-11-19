using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Entities;
using NouAssist.PersonalDevelopment.Habits.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.PersonalDevelopment.Habits.Services
{
    public class HabitAnalyticsService : DomainService
    {
        public HabitMetrics CalculateMetrics(Habit habit, List<HabitTrackingEntry> trackingHistory)
        {
            var completed = trackingHistory.Where(e => e.Completed).ToList();

            return new HabitMetrics
            {
                HabitId = habit.Id,
                CurrentStreak = CalculateCurrentStreak(completed),
                LongestStreak = CalculateLongestStreak(completed),
                CompletionRate = CalculateCompletionRate(trackingHistory),
                Last7DaysRate = CalculateCompletionRateForPeriod(trackingHistory, 7),
                Last30DaysRate = CalculateCompletionRateForPeriod(trackingHistory, 30),
                ConsistencyScore = CalculateConsistencyScore(trackingHistory),
                TotalCompletions = completed.Count,
                TotalAttempts = trackingHistory.Count,
                BestDayOfWeek = GetBestDayOfWeek(trackingHistory),
                IsStrong = false // Will be set based on consistency threshold
            };
        }

        public int CalculateCurrentStreak(List<HabitTrackingEntry> completedEntries)
        {
            if (!completedEntries.Any())
            {
                return 0;
            }

            var sorted = completedEntries.OrderByDescending(e => e.LogDate).ToList();
            int streak = 0;
            DateTime checkDate = Clock.Now.Date;

            foreach (var entry in sorted)
            {
                var normalized = entry.LogDate.Date;
                var checkNormalized = checkDate.Date;

                if (normalized == checkNormalized ||
                    normalized == checkNormalized.AddDays(-1))
                {
                    streak++;
                    checkDate = entry.LogDate.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        public int CalculateLongestStreak(List<HabitTrackingEntry> completedEntries)
        {
            if (!completedEntries.Any())
            {
                return 0;
            }

            var sorted = completedEntries.OrderBy(e => e.LogDate).ToList();
            int maxStreak = 0;
            int currentStreak = 0;
            DateTime? lastDate = null;

            foreach (var entry in sorted)
            {
                if (!lastDate.HasValue)
                {
                    currentStreak = 1;
                }
                else
                {
                    var daysDiff = (entry.LogDate.Date - lastDate.Value.Date).Days;
                    if (daysDiff == 1)
                    {
                        currentStreak++;
                    }
                    else
                    {
                        maxStreak = Math.Max(maxStreak, currentStreak);
                        currentStreak = 1;
                    }
                }
                lastDate = entry.LogDate;
            }

            return Math.Max(maxStreak, currentStreak);
        }

        public double CalculateCompletionRate(List<HabitTrackingEntry> trackingHistory)
        {
            if (!trackingHistory.Any())
            {
                return 0.0;
            }

            var completed = trackingHistory.Count(e => e.Completed);
            return (double)completed / trackingHistory.Count;
        }

        public double CalculateCompletionRateForPeriod(List<HabitTrackingEntry> trackingHistory, int days)
        {
            var cutoffDate = Clock.Now.Date.AddDays(-days);
            var entries = trackingHistory.Where(e => e.LogDate >= cutoffDate).ToList();

            if (!entries.Any())
            {
                return 0.0;
            }

            var completed = entries.Count(e => e.Completed);
            return (double)completed / entries.Count;
        }

        public int CalculateConsistencyScore(List<HabitTrackingEntry> trackingHistory)
        {
            var rate = CalculateCompletionRate(trackingHistory);
            var completedEntries = trackingHistory.Where(e => e.Completed).ToList();
            var currentStreak = CalculateCurrentStreak(completedEntries);

            var streakBonus = Math.Min(currentStreak / 30.0, 0.2);
            var score = (rate + streakBonus) * 100;

            return Math.Min((int)Math.Round(score), 100);
        }

        public int? GetBestDayOfWeek(List<HabitTrackingEntry> trackingHistory)
        {
            var byDay = new Dictionary<int, List<bool>>();

            foreach (var entry in trackingHistory)
            {
                var day = (int)entry.LogDate.DayOfWeek;
                if (!byDay.ContainsKey(day))
                {
                    byDay[day] = new List<bool>();
                }
                byDay[day].Add(entry.Completed);
            }

            if (!byDay.Any())
            {
                return null;
            }

            var rates = byDay.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Count(c => c) / (double)kvp.Value.Count
            );

            return rates.OrderByDescending(kvp => kvp.Value).First().Key;
        }

        public Dictionary<int, double> GetCompletionByDayOfWeek(List<HabitTrackingEntry> trackingHistory)
        {
            var byDay = new Dictionary<int, List<bool>>();

            foreach (var entry in trackingHistory)
            {
                var day = (int)entry.LogDate.DayOfWeek;
                if (!byDay.ContainsKey(day))
                {
                    byDay[day] = new List<bool>();
                }
                byDay[day].Add(entry.Completed);
            }

            return byDay.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Count(c => c) / (double)kvp.Value.Count
            );
        }
    }
}
