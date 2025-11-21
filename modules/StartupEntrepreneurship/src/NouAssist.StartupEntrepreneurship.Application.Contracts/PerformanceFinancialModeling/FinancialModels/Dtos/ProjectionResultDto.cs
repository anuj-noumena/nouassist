using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos
{
    /// <summary>
    /// DTO for ProjectionResult
    /// </summary>
    public class ProjectionResultDto : PerformanceFinancialModelingDto
    {
        public Guid ModelId { get; set; }
        public Guid ScenarioId { get; set; }
        public ProjectionType Type { get; set; }
        public DateTime CalculatedAt { get; set; }
        public string Notes { get; set; }
        public List<ProjectedAmountDto> ProjectedAmounts { get; set; } = new List<ProjectedAmountDto>();
        public string ModelName { get; set; }
        public string ScenarioName { get; set; }
        public decimal TotalProjected { get; set; }
        public decimal AverageProjected { get; set; }
        public int? RunwayMonths { get; set; }
        public DateTime? BreakEvenDate { get; set; }
    }

    /// <summary>
    /// Input DTO for projection calculation
    /// </summary>
    public class CalculateProjectionDto
    {
        [Required]
        public ProjectionType Type { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxNotesLength)]
        public string Notes { get; set; }

        public bool ForceRecalculation { get; set; } = false;
    }
}
