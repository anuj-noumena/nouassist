using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos
{
    /// <summary>
    /// DTO for adding a contingency item to a crisis plan
    /// </summary>
    public class AddContingencyItemDto
    {
        [Required]
        [StringLength(StrategicPlanningConsts.MaxGoalTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(StrategicPlanningConsts.MaxContingencyDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [Range(1, 100)]
        public int Priority { get; set; }

        [Required]
        [StringLength(StrategicPlanningConsts.MaxContingencyDescriptionLength)]
        public string RiskDescription { get; set; }

        [Required]
        public CrisisLevel RiskLevel { get; set; }

        [Required]
        [Range(0.0, 1.0)]
        public decimal ProbabilityScore { get; set; }

        [Required]
        [Range(0.0, 1.0)]
        public decimal ImpactScore { get; set; }
    }
}
