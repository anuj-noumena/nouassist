using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos
{
    /// <summary>
    /// Output DTO for TrackedMetric
    /// </summary>
    public class TrackedMetricDto : PerformanceFinancialModelingFullAuditedDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MetricType Type { get; set; }
        public MetricDataType DataType { get; set; }
        public string Unit { get; set; }
        public MetricFrequency Frequency { get; set; }
        public bool IsActive { get; set; }
        public Guid DefinitionId { get; set; }
        public MetricValueDto LastValue { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int DataPointCount { get; set; }
        public int ActiveGoalCount { get; set; }
        public decimal? TrendPercentage { get; set; }
    }

    /// <summary>
    /// Detailed DTO for TrackedMetric with data points and goals
    /// </summary>
    public class TrackedMetricDetailDto : TrackedMetricDto
    {
        public List<MetricDataPointDto> DataPoints { get; set; } = new List<MetricDataPointDto>();
        public List<MetricGoalDto> Goals { get; set; } = new List<MetricGoalDto>();
        public string DefinitionName { get; set; }
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// Input DTO for creating TrackedMetric
    /// </summary>
    public class CreateTrackedMetricDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxMetricNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxMetricDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public MetricType Type { get; set; }

        [Required]
        public MetricDataType DataType { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength)]
        public string Unit { get; set; }

        [Required]
        public MetricFrequency Frequency { get; set; }

        [Required]
        public Guid DefinitionId { get; set; }
    }

    /// <summary>
    /// Input DTO for updating TrackedMetric
    /// </summary>
    public class UpdateTrackedMetricDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxMetricNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxMetricDescriptionLength)]
        public string Description { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength)]
        public string Unit { get; set; }
    }

    /// <summary>
    /// Input DTO for filtering TrackedMetric list
    /// </summary>
    public class GetTrackedMetricListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public MetricType? Type { get; set; }
        public bool? IsActive { get; set; }
        public Guid? DefinitionId { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? LastUpdatedSince { get; set; }
        public bool? HasActiveGoals { get; set; }
    }

    /// <summary>
    /// Input DTO for adding data point
    /// </summary>
    public class AddDataPointDto
    {
        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }

    /// <summary>
    /// Input DTO for setting goal
    /// </summary>
    public class SetMetricGoalDto
    {
        [Required]
        public MetricValueDto TargetValue { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }

        [Required]
        public ThresholdType ThresholdType { get; set; }

        public MetricValueDto SecondaryValue { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Description { get; set; }
    }
}
