using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos
{
    /// <summary>
    /// DTO for ModelScenario
    /// </summary>
    public class ModelScenarioDto : PerformanceFinancialModelingDto
    {
        public Guid ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ScenarioType Type { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> AssociatedAssumptionSets { get; set; } = new List<Guid>();
        public List<string> AssumptionSetNames { get; set; } = new List<string>();
        public int ProjectionCount { get; set; }
        public DateTime? LastCalculatedAt { get; set; }
    }

    /// <summary>
    /// Input DTO for creating ModelScenario
    /// </summary>
    public class CreateModelScenarioDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxScenarioNameLength)]
        public string Name { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxScenarioDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public ScenarioType Type { get; set; }
    }

    /// <summary>
    /// Input DTO for updating ModelScenario
    /// </summary>
    public class UpdateModelScenarioDto
    {
        [Required]
        [StringLength(PerformanceFinancialModelingConsts.MaxScenarioNameLength)]
        public string Name { get; set; }

        [StringLength(PerformanceFinancialModelingConsts.MaxScenarioDescriptionLength)]
        public string Description { get; set; }
    }

    /// <summary>
    /// Input DTO for associating assumption sets with scenario
    /// </summary>
    public class AssociateAssumptionSetsDto
    {
        public List<Guid> AssumptionSetIds { get; set; } = new List<Guid>();
    }
}
