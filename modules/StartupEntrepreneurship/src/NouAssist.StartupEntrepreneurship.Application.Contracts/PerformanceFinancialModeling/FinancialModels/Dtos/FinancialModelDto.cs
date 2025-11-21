using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos
{
    /// <summary>
    /// Output DTO for FinancialModel
    /// </summary>
    public class FinancialModelDto : PerformanceFinancialModelingFullAuditedDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ModelStatus Status { get; set; }
        public int ProjectionYears { get; set; }
        public DateTime BaseDate { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public int ScenarioCount { get; set; }
        public int AssumptionSetCount { get; set; }
        public int ProjectionCount { get; set; }
        public DateTime? LastCalculatedAt { get; set; }
        public bool CanCalculate { get; set; }
    }

    /// <summary>
    /// Detailed DTO for FinancialModel with scenarios, assumptions, and projections
    /// </summary>
    public class FinancialModelDetailDto : FinancialModelDto
    {
        public List<ModelScenarioDto> Scenarios { get; set; } = new List<ModelScenarioDto>();
        public List<AssumptionSetDto> AssumptionSets { get; set; } = new List<AssumptionSetDto>();
        public List<ProjectionResultDto> Projections { get; set; } = new List<ProjectionResultDto>();
    }

    /// <summary>
    /// Input DTO for creating FinancialModel
    /// </summary>
    public class CreateFinancialModelDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxModelNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxModelDescriptionLength)]
        public string Description { get; set; }

        [Range(1, PerformanceFinancialModelingConsts.MaxProjectionYears)]
        public int ProjectionYears { get; set; }

        [Required]
        public DateTime BaseDate { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "USD";
    }

    /// <summary>
    /// Input DTO for updating FinancialModel
    /// </summary>
    public class UpdateFinancialModelDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxModelNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxModelDescriptionLength)]
        public string Description { get; set; }

        [Range(1, PerformanceFinancialModelingConsts.MaxProjectionYears)]
        public int ProjectionYears { get; set; }
    }

    /// <summary>
    /// Input DTO for changing model status
    /// </summary>
    public class ChangeModelStatusDto
    {
        [Required]
        public ModelStatus Status { get; set; }
    }

    /// <summary>
    /// Input DTO for filtering FinancialModel list
    /// </summary>
    public class GetFinancialModelListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ModelStatus? Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public bool? CanCalculate { get; set; }
    }
}
