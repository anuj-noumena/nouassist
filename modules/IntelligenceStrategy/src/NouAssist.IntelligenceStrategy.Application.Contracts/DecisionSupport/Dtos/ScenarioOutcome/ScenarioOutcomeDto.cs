using System;

namespace NouAssist.DecisionSupport.Dtos.ScenarioOutcome
{
    /// <summary>
    /// DTO for scenario outcome data
    /// </summary>
    public class ScenarioOutcomeDto
    {
        /// <summary>
        /// The ID of the scenario model this outcome belongs to
        /// </summary>
        public Guid ScenarioModelId { get; set; }
        
        /// <summary>
        /// The area or aspect this outcome affects
        /// </summary>
        public string Area { get; set; }
        
        /// <summary>
        /// The description of the outcome
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The impact level of this outcome (1-5)
        /// </summary>
        public int ImpactValue { get; set; }
        
        /// <summary>
        /// Optional description of the impact
        /// </summary>
        public string ImpactDescription { get; set; }
    }
}
