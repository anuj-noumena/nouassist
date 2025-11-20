using System;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for displaying analysis reference information
    /// </summary>
    public class AnalysisReferenceDto
    {
        /// <summary>
        /// The strategic initiative that this analysis supports
        /// </summary>
        public Guid StrategicInitiativeId { get; set; }
        
        /// <summary>
        /// Type of analysis (e.g., "SWOT", "ROI", "RiskAssessment")
        /// </summary>
        public string ReferenceType { get; set; }
        
        /// <summary>
        /// Key or ID to locate the reference
        /// </summary>
        public string ReferenceKey { get; set; }
        
        /// <summary>
        /// Description of the analysis and how it supports the initiative
        /// </summary>
        public string Description { get; set; }
    }
}
