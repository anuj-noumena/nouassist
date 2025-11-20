using System;

namespace NouAssist.DecisionSupport.Dtos.Score
{
    /// <summary>
    /// DTO for alternative score data
    /// </summary>
    public class AlternativeScoreDto
    {
        /// <summary>
        /// The criterion this score is for
        /// </summary>
        public Guid CriterionId { get; set; }
        
        /// <summary>
        /// Name of the criterion for display purposes
        /// </summary>
        public string CriterionName { get; set; }
        
        /// <summary>
        /// The score value (0-10)
        /// </summary>
        public decimal Score { get; set; }
        
        /// <summary>
        /// The weighted score (Score * Criterion Weight)
        /// </summary>
        public decimal WeightedScore { get; set; }
        
        /// <summary>
        /// Justification for the score
        /// </summary>
        public string Justification { get; set; }
    }
}
