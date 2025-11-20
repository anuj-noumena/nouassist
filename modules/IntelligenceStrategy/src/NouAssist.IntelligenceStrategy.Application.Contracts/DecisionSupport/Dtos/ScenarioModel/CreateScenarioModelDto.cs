using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.DecisionSupport.Enums;

namespace NouAssist.DecisionSupport.Dtos.ScenarioModel
{
    /// <summary>
    /// DTO for creating a scenario model
    /// </summary>
    public class CreateScenarioModelDto
    {
        /// <summary>
        /// The ID of the scenario analysis this model belongs to
        /// </summary>
        [Required]
        public Guid ScenarioAnalysisId { get; set; }
        
        /// <summary>
        /// The name of the scenario
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// The title of the scenario description
        /// </summary>
        [Required]
        public string Title { get; set; }
        
        /// <summary>
        /// The detailed description of the scenario
        /// </summary>
        [Required]
        public string Description { get; set; }
        
        /// <summary>
        /// The key assumptions of the scenario
        /// </summary>
        [Required]
        public string Assumptions { get; set; }
        
        /// <summary>
        /// The likelihood category of this scenario occurring
        /// </summary>
        [Required]
        public ScenarioLikelihood Likelihood { get; set; }
        
        /// <summary>
        /// The confidence level in the likelihood assessment (0-1)
        /// </summary>
        [Range(0, 1)]
        public decimal ConfidenceLevel { get; set; }
        
        /// <summary>
        /// The rationale for the likelihood assessment
        /// </summary>
        [Required]
        public string LikelihoodRationale { get; set; }
    }
}
