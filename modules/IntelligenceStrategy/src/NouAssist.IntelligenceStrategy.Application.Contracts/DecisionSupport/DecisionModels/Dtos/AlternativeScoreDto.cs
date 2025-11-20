using System;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for displaying alternative score details
    /// </summary>
    public class AlternativeScoreDto
    {
        /// <summary>
        /// The alternative being scored
        /// </summary>
        public Guid AlternativeId { get; set; }
        
        /// <summary>
        /// The criterion being scored against
        /// </summary>
        public Guid CriterionId { get; set; }
        
        /// <summary>
        /// Score value (normalized between 0 and 1)
        /// </summary>
        public decimal Score { get; set; }
        
        /// <summary>
        /// Weighted score (score * criterion weight)
        /// </summary>
        public decimal WeightedScore { get; set; }
    }
}
