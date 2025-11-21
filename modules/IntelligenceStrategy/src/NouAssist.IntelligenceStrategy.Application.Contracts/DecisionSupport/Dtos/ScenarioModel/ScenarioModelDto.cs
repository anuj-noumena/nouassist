using System;
using System.Collections.Generic;
using NouAssist.DecisionSupport.Dtos.ScenarioOutcome;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.ScenarioModel
{
    /// <summary>
    /// DTO for scenario model data
    /// </summary>
    public class ScenarioModelDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the scenario analysis this model belongs to
        /// </summary>
        public Guid ScenarioAnalysisId { get; set; }
        
        /// <summary>
        /// The name of the scenario
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The title of the scenario description
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The detailed description of the scenario
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The key assumptions of the scenario
        /// </summary>
        public string Assumptions { get; set; }
        
        /// <summary>
        /// The likelihood category of this scenario occurring
        /// </summary>
        public ScenarioLikelihood Likelihood { get; set; }
        
        /// <summary>
        /// The confidence level in the likelihood assessment (0-1)
        /// </summary>
        public decimal ConfidenceLevel { get; set; }
        
        /// <summary>
        /// The rationale for the likelihood assessment
        /// </summary>
        public string LikelihoodRationale { get; set; }
        
        /// <summary>
        /// The potential outcomes of this scenario
        /// </summary>
        public List<ScenarioOutcomeDto> Outcomes { get; set; }
        
        /// <summary>
        /// The average impact score of all outcomes
        /// </summary>
        public decimal AverageImpact { get; set; }
        
        public ScenarioModelDto()
        {
            Outcomes = new List<ScenarioOutcomeDto>();
        }
    }
}
