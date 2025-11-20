using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanning.Dtos
{
    /// <summary>
    /// DTO for adding a capability roadmap item
    /// </summary>
    public class AddCapabilityRoadmapItemDto
    {
        [Required]
        [StringLength(100)]
        public string SkillName { get; set; }

        [Required]
        [StringLength(50)]
        public string SkillCategory { get; set; }

        [StringLength(500)]
        public string SkillDescription { get; set; }

        [Required]
        public CapabilityLevel CurrentLevel { get; set; }

        [Required]
        public CapabilityLevel TargetLevel { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal InvestmentRequired { get; set; }
    }
}
