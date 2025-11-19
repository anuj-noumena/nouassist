using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using NouAssist.PersonalDevelopment.Habits.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.PersonalDevelopment.Habits.Services
{
    /// <summary>
    /// Domain service for analyzing wellness patterns across health, energy, mood, and focus tracking
    /// </summary>
    public class WellnessAnalyticsService : DomainService
    {
        private readonly IHealthTrackingRepository _healthRepository;
        private readonly IEnergyTrackingRepository _energyRepository;
        private readonly IMoodTrackingRepository _moodRepository;
        private readonly IFocusSessionRepository _focusRepository;

        public WellnessAnalyticsService(
            IHealthTrackingRepository healthRepository,
            IEnergyTrackingRepository energyRepository,
            IMoodTrackingRepository moodRepository,
            IFocusSessionRepository focusRepository
        )
        {
            _healthRepository = healthRepository;
            _energyRepository = energyRepository;
            _moodRepository = moodRepository;
            _focusRepository = focusRepository;
        }

        public async Task<WellnessReport> GenerateWellnessReportAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate
        )
        {
            var healthEntries = await _healthRepository.GetByOwnerInPeriodAsync(ownerId, startDate, endDate);
            var energyEntries = await _energyRepository.GetByOwnerInPeriodAsync(ownerId, startDate, endDate);
            var moodEntries = await _moodRepository.GetByOwnerInPeriodAsync(ownerId, startDate, endDate);
            var focusSessions = await _focusRepository.GetByOwnerInPeriodAsync(ownerId, startDate, endDate);

            var report = new WellnessReport
            {
                OwnerId = ownerId,
                PeriodStart = startDate,
                PeriodEnd = endDate,
                GeneratedAt = Clock.Now
            };

            // Health Metrics
            if (healthEntries.Any())
            {
                report.AverageSleepHours = healthEntries.Average(e => e.SleepHours);
                report.AverageWaterIntake = healthEntries.Average(e => e.WaterCount);
                report.DaysWithGoodSleep = healthEntries.Count(e => e.HasGoodSleep);
                report.DaysWithAdequateWater = healthEntries.Count(e => e.HasAdequateWater);
                report.TotalPostureChecks = healthEntries.Sum(e => e.PostureChecks);
                report.TotalBreaksTaken = healthEntries.Sum(e => e.BreaksTaken);
            }

            // Energy Metrics
            if (energyEntries.Any())
            {
                report.AverageEnergyLevel = energyEntries.Average(e => e.EnergyLevel);
                report.HighEnergyDays = energyEntries.Count(e => e.Level == Enums.EnergyLevel.High);
                report.LowEnergyDays = energyEntries.Count(e => e.Level == Enums.EnergyLevel.Low);
            }

            // Mood Metrics
            if (moodEntries.Any())
            {
                report.AverageMoodRating = moodEntries.Average(e => e.MoodRating);
                report.PositiveMoodDays = moodEntries.Count(e => e.MoodRating >= 7);
                report.NegativeMoodDays = moodEntries.Count(e => e.MoodRating <= 4);
                report.MoodTrend = CalculateTrend(moodEntries.OrderBy(e => e.Date).Select(e => (double)e.MoodRating).ToList());
            }

            // Focus Metrics
            if (focusSessions.Any())
            {
                report.TotalPomodoros = focusSessions.Sum(s => s.CompletedPomodoros);
                report.TotalFocusMinutes = focusSessions.Sum(s => s.TotalFocusTime);
                report.AveragePomodorosPerDay = (double)report.TotalPomodoros / focusSessions.Count;
                report.MostProductiveDay = GetMostProductiveDay(focusSessions);
            }

            // Overall Wellness Score (0-100)
            report.OverallWellnessScore = CalculateOverallWellnessScore(report);

            // Correlations
            report.Correlations = AnalyzeCorrelations(healthEntries, energyEntries, moodEntries, focusSessions);

            // Recommendations
            report.Recommendations = GenerateRecommendations(report);

            return report;
        }

        private string CalculateTrend(List<double> values)
        {
            if (values.Count < 2)
            {
                return "Stable";
            }

            var firstHalf = values.Take(values.Count / 2).Average();
            var secondHalf = values.Skip(values.Count / 2).Average();

            var difference = secondHalf - firstHalf;

            if (difference > 0.5)
            {
                return "Improving";
            }

            if (difference < -0.5)
            {
                return "Declining";
            }

            return "Stable";
        }

        private DateTime? GetMostProductiveDay(List<FocusSession> sessions)
        {
            if (!sessions.Any())
            {
                return null;
            }

            return sessions
                .OrderByDescending(s => s.CompletedPomodoros)
                .ThenByDescending(s => s.TotalFocusTime)
                .First()
                .Date;
        }

        private int CalculateOverallWellnessScore(WellnessReport report)
        {
            double score = 0;
            int components = 0;

            // Sleep score (25 points)
            if (report.AverageSleepHours > 0)
            {
                var sleepScore = Math.Min(report.AverageSleepHours / 8.0, 1.0) * 25;
                score += sleepScore;
                components++;
            }

            // Energy score (25 points)
            if (report.AverageEnergyLevel > 0)
            {
                var energyScore = (report.AverageEnergyLevel / 10.0) * 25;
                score += energyScore;
                components++;
            }

            // Mood score (25 points)
            if (report.AverageMoodRating > 0)
            {
                var moodScore = (report.AverageMoodRating / 10.0) * 25;
                score += moodScore;
                components++;
            }

            // Focus score (25 points)
            if (report.TotalPomodoros > 0)
            {
                var expectedPomodoros = (report.PeriodEnd - report.PeriodStart).Days * 8; // 8 pomodoros per day
                var focusScore = Math.Min((double)report.TotalPomodoros / expectedPomodoros, 1.0) * 25;
                score += focusScore;
                components++;
            }

            return components > 0 ? (int)Math.Round(score) : 0;
        }

        private Dictionary<string, string> AnalyzeCorrelations(
            List<HealthTrackingEntry> health,
            List<EnergyTrackingEntry> energy,
            List<MoodTrackingEntry> mood,
            List<FocusSession> focus
        )
        {
            var correlations = new Dictionary<string, string>();

            // Sleep-Energy correlation
            if (health.Any() && energy.Any())
            {
                var correlation = CalculateSimpleCorrelation(
                    health.Select(h => (double)h.SleepHours).ToList(),
                    energy.Select(e => (double)e.EnergyLevel).ToList()
                );
                correlations["SleepEnergy"] = correlation > 0.5 ? "Strong positive" : 
                                             correlation < -0.5 ? "Strong negative" : "Weak";
            }

            // Mood-Productivity correlation
            if (mood.Any() && focus.Any())
            {
                var correlation = CalculateSimpleCorrelation(
                    mood.Select(m => (double)m.MoodRating).ToList(),
                    focus.Select(f => (double)f.CompletedPomodoros).ToList()
                );
                correlations["MoodProductivity"] = correlation > 0.5 ? "Strong positive" : 
                                                   correlation < -0.5 ? "Strong negative" : "Weak";
            }

            return correlations;
        }

        private double CalculateSimpleCorrelation(List<double> x, List<double> y)
        {
            if (x.Count != y.Count || x.Count == 0)
            {
                return 0;
            }

            var avgX = x.Average();
            var avgY = y.Average();

            var numerator = x.Zip(y, (xi, yi) => (xi - avgX) * (yi - avgY)).Sum();
            var denomX = Math.Sqrt(x.Sum(xi => Math.Pow(xi - avgX, 2)));
            var denomY = Math.Sqrt(y.Sum(yi => Math.Pow(yi - avgY, 2)));

            return denomX * denomY == 0 ? 0 : numerator / (denomX * denomY);
        }

        private List<string> GenerateRecommendations(WellnessReport report)
        {
            var recommendations = new List<string>();

            if (report.AverageSleepHours < 7)
            {
                recommendations.Add("💤 Consider getting more sleep - aim for 7-9 hours per night");
            }

            if (report.AverageWaterIntake < 8)
            {
                recommendations.Add("💧 Increase water intake - target 8 glasses per day");
            }

            if (report.AverageEnergyLevel < 5)
            {
                recommendations.Add("⚡ Low energy detected - review sleep quality and consider exercise");
            }

            if (report.AverageMoodRating < 6)
            {
                recommendations.Add("😊 Focus on mood improvement - try gratitude journaling or social activities");
            }

            if (report.TotalPomodoros < (report.PeriodEnd - report.PeriodStart).Days * 4)
            {
                recommendations.Add("🎯 Increase focused work sessions - aim for 4-6 pomodoros per day");
            }

            if (report.TotalBreaksTaken < (report.PeriodEnd - report.PeriodStart).Days)
            {
                recommendations.Add("🧘 Take more breaks throughout the day for better productivity");
            }

            if (report.MoodTrend == "Declining")
            {
                recommendations.Add("⚠️ Mood declining - consider stress management techniques");
            }

            return recommendations;
        }
    }
}
