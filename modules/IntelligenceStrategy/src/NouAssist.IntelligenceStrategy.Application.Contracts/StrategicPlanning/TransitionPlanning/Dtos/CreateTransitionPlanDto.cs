using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanning.Dtos
{
    /// <summary>
    /// DTO for creating a new Transition Plan
    /// </summary>
    public class CreateTransitionPlanDto
    {
        [Required]
        [StringLength(150)]
        public string PlanName { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public TransitionType Type { get; set; }

        [Required]
        public DateTime PlannedStartDate { get; set; }

        [Required]
        public DateTime EstimatedCompletionDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal EstimatedCost { get; set; }
    }
}
