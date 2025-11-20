using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos
{
    /// <summary>
    /// DTO for adding a target to a Strategic Goal
    /// </summary>
    public class AddGoalTargetDto
    {
        [Required]
        [StringLength(StrategicPlanningConsts.MaxMetricNameLength)]
        public string MetricName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TargetValue { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }

        [StringLength(StrategicPlanningConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }
}
