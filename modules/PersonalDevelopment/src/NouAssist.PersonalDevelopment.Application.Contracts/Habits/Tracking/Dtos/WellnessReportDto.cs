using System;
using System.Collections.Generic;

namespace NouAssist.PersonalDevelopment.Habits.Tracking.Dtos
{
    public class WellnessReportDto
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime GeneratedAt { get; set; }

        // Health Metrics
        public double AverageSleepHours { get; set; }
        public double AverageWaterIntake { get; set; }
        public int DaysWithGoodSleep { get; set; }
        public int DaysWithAdequateWater { get; set; }
        public int TotalPostureChecks { get; set; }
        public int TotalBreaksTaken { get; set; }

        // Energy Metrics
        public double AverageEnergyLevel { get; set; }
        public int HighEnergyDays { get; set; }
        public int LowEnergyDays { get; set; }

        // Mood Metrics
        public double AverageMoodRating { get; set; }
        public int PositiveMoodDays { get; set; }
        public int NegativeMoodDays { get; set; }
        public string MoodTrend { get; set; }

        // Focus Metrics
        public int TotalPomodoros { get; set; }
        public int TotalFocusMinutes { get; set; }
        public double AveragePomodorosPerDay { get; set; }
        public DateTime? MostProductiveDay { get; set; }

        // Overall
        public int OverallWellnessScore { get; set; }
        public Dictionary<string, string> Correlations { get; set; }
        public List<string> Recommendations { get; set; }
    }

    public class GetWellnessReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
