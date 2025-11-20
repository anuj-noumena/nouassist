using System;

namespace NouAssist.LearningDevelopment.Integration.LearningIntegration.Dtos
{
    public class LearningDashboardDto
    {
        // Learning Progress
        public int ActiveLearningPaths { get; set; }
        public int CompletedLearningPaths { get; set; }
        public double AverageMasteryScore { get; set; }

        // Task Metrics
        public int TasksCompleted { get; set; }
        public int TasksInProgress { get; set; }
        public int TasksOverdue { get; set; }
        public double TaskCompletionRate { get; set; }

        // Time Metrics
        public double TotalHoursInvested { get; set; }
        public int ScheduledHoursThisWeek { get; set; }
        public double ActualVsPlannedTimePercentage { get; set; }

        // Productivity Insights
        public string MostProductiveTimeOfDay { get; set; }
        public int AverageSessionLengthMinutes { get; set; }
        public int CurrentStreak { get; set; }

        // Recommendations
        public RecommendedAction RecommendedNextAction { get; set; }
    }

    public class RecommendedAction
    {
        public string Type { get; set; }
        public Guid? LearningPathId { get; set; }
        public string Phase { get; set; }
        public Guid? TaskInstanceId { get; set; }
        public Guid? TimeblockId { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public string Message { get; set; }
    }
}
