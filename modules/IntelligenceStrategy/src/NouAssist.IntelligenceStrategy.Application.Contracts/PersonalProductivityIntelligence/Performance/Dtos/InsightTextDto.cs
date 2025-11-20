using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for insight text
    /// </summary>
    public class InsightTextDto
    {
        /// <summary>
        /// The insight text
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// Category of the insight
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Relevance score (0-1)
        /// </summary>
        public double? RelevanceScore { get; set; }
    }
}
