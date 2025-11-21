using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions.Dtos
{
    /// <summary>
    /// Output DTO for MetricDefinition
    /// </summary>
    public class MetricDefinitionDto : PerformanceFinancialModelingFullAuditedDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MetricType Type { get; set; }
        public MetricDataType DataType { get; set; }
        public string Unit { get; set; }
        public CalculationRuleDto CalculationRule { get; set; }
        public bool IsActive { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UsageCount { get; set; }
    }

    /// <summary>
    /// Input DTO for creating MetricDefinition
    /// </summary>
    public class CreateMetricDefinitionDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxDefinitionNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public MetricType Type { get; set; }

        [Required]
        public MetricDataType DataType { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength)]
        public string Unit { get; set; }

        [Required]
        public CalculationRuleDto CalculationRule { get; set; }

        public Guid? CategoryId { get; set; }
    }

    /// <summary>
    /// Input DTO for updating MetricDefinition
    /// </summary>
    public class UpdateMetricDefinitionDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxDefinitionNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public CalculationRuleDto CalculationRule { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength)]
        public string Unit { get; set; }

        public Guid? CategoryId { get; set; }
    }

    /// <summary>
    /// Input DTO for filtering MetricDefinition list
    /// </summary>
    public class GetMetricDefinitionListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public MetricType? Type { get; set; }
        public bool? IsActive { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? IsAutomated { get; set; }
    }
}
