using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos
{
    /// <summary>
    /// DTO for MetricGoal
    /// </summary>
    public class MetricGoalDto : PerformanceFinancialModelingDto
    {
        public Guid MetricId { get; set; }
        public GoalTargetDto Target { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsAchieved { get; set; }
        public DateTime? AchievedAt { get; set; }
        public int DaysRemaining { get; set; }
        public decimal? ProgressPercentage { get; set; }
        public bool IsOnTrack { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Input DTO for updating goal
    /// </summary>
    public class UpdateMetricGoalDto
    {
        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Description { get; set; }
    }
}
