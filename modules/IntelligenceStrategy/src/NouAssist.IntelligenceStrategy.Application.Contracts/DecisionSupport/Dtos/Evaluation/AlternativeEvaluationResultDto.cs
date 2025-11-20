using System;

namespace NouAssist.DecisionSupport.Dtos.Evaluation
{
    /// <summary>
    /// DTO for alternative evaluation results
    /// </summary>
    public class AlternativeEvaluationResultDto
    {
        /// <summary>
        /// The alternative ID
        /// </summary>
        public Guid AlternativeId { get; set; }
        
        /// <summary>
        /// The name of the alternative
        /// </summary>
        public string AlternativeName { get; set; }
        
        /// <summary>
        /// The total score of the alternative
        /// </summary>
        public decimal TotalScore { get; set; }
        
        /// <summary>
        /// The rank of the alternative (1 is highest)
        /// </summary>
        public int Rank { get; set; }
    }
}
