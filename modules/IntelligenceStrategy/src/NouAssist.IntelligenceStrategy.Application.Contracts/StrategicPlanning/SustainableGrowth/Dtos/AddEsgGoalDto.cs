using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos
{
    /// <summary>
    /// DTO for adding an ESG goal to a framework
    /// </summary>
    public class AddEsgGoalDto
    {
        [Required]
        public EsgCategory Category { get; set; }

        [Required]
        [StringLength(StrategicPlanningConsts.MaxGoalTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(StrategicPlanningConsts.MaxEsgGoalDescriptionLength)]
        public string Description { get; set; }

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
    }
}
