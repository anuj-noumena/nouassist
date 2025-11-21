using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos
{
    /// <summary>
    /// DTO for AssumptionSet
    /// </summary>
    public class AssumptionSetDto : PerformanceFinancialModelingDto
    {
        public Guid ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<AssumptionValueDto> Assumptions { get; set; } = new List<AssumptionValueDto>();
        public int AssumptionCount { get; set; }
        public bool IsValidated { get; set; }
    }

    /// <summary>
    /// Input DTO for creating AssumptionSet
    /// </summary>
    public class CreateAssumptionSetDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxAssumptionNameLength)]
        public string Name { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxAssumptionDescriptionLength)]
        public string Description { get; set; }

        public List<AssumptionValueDto> Assumptions { get; set; } = new List<AssumptionValueDto>();
    }

    /// <summary>
    /// Input DTO for updating AssumptionSet
    /// </summary>
    public class UpdateAssumptionSetDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxAssumptionNameLength)]
        public string Name { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxAssumptionDescriptionLength)]
        public string Description { get; set; }

        public List<AssumptionValueDto> Assumptions { get; set; } = new List<AssumptionValueDto>();
    }

    /// <summary>
    /// Input DTO for updating specific assumption
    /// </summary>
    public class UpdateAssumptionDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
