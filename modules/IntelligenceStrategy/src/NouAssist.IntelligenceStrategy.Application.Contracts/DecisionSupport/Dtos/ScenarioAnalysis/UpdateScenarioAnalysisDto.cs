using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.ScenarioAnalysis
{
    /// <summary>
    /// DTO for updating a scenario analysis
    /// </summary>
    public class UpdateScenarioAnalysisDto
    {
        /// <summary>
        /// The title of the scenario analysis
        /// </summary>
        [Required]
        public string Title { get; set; }
        
        /// <summary>
        /// The purpose or focus question of the analysis
        /// </summary>
        [Required]
        public string Purpose { get; set; }
        
        /// <summary>
        /// The time horizon for the scenario analysis
        /// </summary>
        [Required]
        public string TimeHorizon { get; set; }
        
        /// <summary>
        /// The scope of the analysis
        /// </summary>
        [Required]
        public string Scope { get; set; }
        
        /// <summary>
        /// Key drivers or factors influencing the scenarios
        /// </summary>
        [Required]
        public string KeyDrivers { get; set; }
    }
}
