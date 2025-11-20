using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.ScenarioOutcome
{
    /// <summary>
    /// DTO for creating a scenario outcome
    /// </summary>
    public class CreateScenarioOutcomeDto
    {
        /// <summary>
        /// The ID of the scenario model this outcome belongs to
        /// </summary>
        [Required]
        public Guid ScenarioModelId { get; set; }
        
        /// <summary>
        /// The area or aspect this outcome affects
        /// </summary>
        [Required]
        public string Area { get; set; }
        
        /// <summary>
        /// The description of the outcome
        /// </summary>
        [Required]
        public string Description { get; set; }
        
        /// <summary>
        /// The impact level of this outcome (1-5)
        /// </summary>
        [Range(1, 5)]
        public int ImpactValue { get; set; }
        
        /// <summary>
        /// Optional description of the impact
        /// </summary>
        public string ImpactDescription { get; set; }
    }
}
