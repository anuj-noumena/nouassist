using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos
{
    /// <summary>
    /// DTO for creating a new Crisis Management Plan
    /// </summary>
    public class CreateCrisisManagementPlanDto
    {
        [Required]
        [StringLength(StrategicPlanningConsts.MaxPlanNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(StrategicPlanningConsts.MaxContingencyDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public CrisisLevel MaxCrisisLevel { get; set; }

        [Required]
        public DateTime NextReviewDate { get; set; }
    }
}
