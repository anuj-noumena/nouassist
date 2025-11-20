using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos
{
    /// <summary>
    /// DTO for creating a new Strategic Goal
    /// </summary>
    public class CreateStrategicGoalDto
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
