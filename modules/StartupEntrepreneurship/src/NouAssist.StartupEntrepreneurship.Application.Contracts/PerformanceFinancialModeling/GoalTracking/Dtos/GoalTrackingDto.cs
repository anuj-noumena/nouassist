using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.GoalTracking.Dtos
{
    /// <summary>
    /// DTO for goal progress
    /// </summary>
    public class GoalProgressDto
    {
        public Guid GoalId { get; set; }
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public decimal ProgressPercentage { get; set; }
        public bool IsOnTrack { get; set; }
        public int DaysRemaining { get; set; }
        public MetricValueDto CurrentValue { get; set; }
        public MetricValueDto TargetValue { get; set; }
        public decimal ProjectedValue { get; set; }
        public string Status { get; set; }
        public DateTime? AchievedAt { get; set; }
        public bool IsOverdue { get; set; }
    }

    /// <summary>
    /// DTO for goal alerts
    /// </summary>
    public class GoalAlertDto
    {
        public Guid GoalId { get; set; }
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
        public string AlertType { get; set; }
        public string Message { get; set; }
        public DateTime AlertDate { get; set; }
        public int Priority { get; set; }
        public bool IsResolved { get; set; }
    }

    /// <summary>
    /// DTO for goal summary
    /// </summary>
    public class GoalSummaryDto
    {
        public int TotalGoals { get; set; }
        public int ActiveGoals { get; set; }
        public int AchievedGoals { get; set; }
        public int OverdueGoals { get; set; }
        public decimal AverageProgress { get; set; }
        public int GoalsOnTrack { get; set; }
        public int GoalsAtRisk { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    /// <summary>
    /// DTO for goal dashboard
    /// </summary>
    public class GoalDashboardDto
    {
        public GoalSummaryDto Summary { get; set; }
        public List<GoalProgressDto> RecentlyAchieved { get; set; } = new List<GoalProgressDto>();
        public List<GoalProgressDto> UpcomingDeadlines { get; set; } = new List<GoalProgressDto>();
        public List<GoalProgressDto> AtRiskGoals { get; set; } = new List<GoalProgressDto>();
        public List<GoalAlertDto> RecentAlerts { get; set; } = new List<GoalAlertDto>();
    }
}
