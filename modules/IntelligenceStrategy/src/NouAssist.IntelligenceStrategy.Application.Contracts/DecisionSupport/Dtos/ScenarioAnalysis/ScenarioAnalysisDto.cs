using System;
using System.Collections.Generic;
using NouAssist.DecisionSupport.Dtos.ScenarioModel;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.ScenarioAnalysis
{
    /// <summary>
    /// DTO for scenario analysis data
    /// </summary>
    public class ScenarioAnalysisDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// The title of the scenario analysis
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The purpose or focus question of the analysis
        /// </summary>
        public string Purpose { get; set; }
        
        /// <summary>
        /// The time horizon for the scenario analysis
        /// </summary>
        public string TimeHorizon { get; set; }
        
        /// <summary>
        /// The scope of the analysis
        /// </summary>
        public string Scope { get; set; }
        
        /// <summary>
        /// Key drivers or factors influencing the scenarios
        /// </summary>
        public string KeyDrivers { get; set; }
        
        /// <summary>
        /// ID of the decision model this analysis is linked to, if any
        /// </summary>
        public Guid? DecisionModelId { get; set; }
        
        /// <summary>
        /// The scenario models being analyzed
        /// </summary>
        public List<ScenarioModelDto> ScenarioModels { get; set; }
        
        /// <summary>
        /// The most likely scenario based on likelihood assessments
        /// </summary>
        public ScenarioModelDto MostLikelyScenario { get; set; }
        
        /// <summary>
        /// The highest impact scenario, regardless of likelihood
        /// </summary>
        public ScenarioModelDto HighestImpactScenario { get; set; }
        
        public ScenarioAnalysisDto()
        {
            ScenarioModels = new List<ScenarioModelDto>();
        }
    }
}
