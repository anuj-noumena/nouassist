using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos
{
    /// <summary>
    /// DTO for updating a Strategic Goal
    /// </summary>
    public class UpdateStrategicGoalDto
    {
        [Required]
        [StringLength(StrategicPlanningConsts.MaxGoalTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(StrategicPlanningConsts.MaxGoalDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public GoalPriority Priority { get; set; }

        [Required]
        public PlanningHorizon Horizon { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }

        public Guid? OwnerId { get; set; }

        [StringLength(StrategicPlanningConsts.MaxNotesLength)]
        public string Notes { get; set; }
    }
}
