using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos
{
    /// <summary>
    /// DTO for creating a new Sustainable Growth Framework
    /// </summary>
    public class CreateSustainableGrowthFrameworkDto
    {
        [Required]
        [StringLength(StrategicPlanningConsts.MaxFrameworkNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(StrategicPlanningConsts.MaxEsgGoalDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public DateTime FrameworkStartDate { get; set; }

        [Required]
        public DateTime FrameworkEndDate { get; set; }
    }
}
