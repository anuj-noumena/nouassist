using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanning.Dtos
{
    /// <summary>
    /// DTO for creating a new Integrated Horizon Plan
    /// </summary>
    public class CreateIntegratedHorizonPlanDto
    {
        [Required]
        [StringLength(150)]
        public string PlanName { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public DateTime PlanStartDate { get; set; }

        [Required]
        public DateTime PlanEndDate { get; set; }

        [Required]
        public PlanningHorizon PrimaryHorizon { get; set; }

        [Required]
        [StringLength(1000)]
        public string VisionStatement { get; set; }
    }
}
