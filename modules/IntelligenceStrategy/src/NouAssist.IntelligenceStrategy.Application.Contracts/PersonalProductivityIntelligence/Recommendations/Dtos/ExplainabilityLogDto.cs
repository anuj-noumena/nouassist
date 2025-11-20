using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Recommendations.Dtos
{
    /// <summary>
    /// DTO for explainability logs
    /// </summary>
    public class ExplainabilityLogDto
    {
        /// <summary>
        /// Name of the factor
        /// </summary>
        public string FactorName { get; set; }
        
        /// <summary>
        /// Human-readable explanation
        /// </summary>
        public string Explanation { get; set; }
        
        /// <summary>
        /// Impact of this factor (-1 to 1)
        /// </summary>
        public double Impact { get; set; }
        
        /// <summary>
        /// Type of evidence
        /// </summary>
        public string EvidenceType { get; set; }
    }
}
