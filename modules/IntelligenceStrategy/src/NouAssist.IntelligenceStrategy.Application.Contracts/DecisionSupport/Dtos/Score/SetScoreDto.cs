using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.Score
{
    /// <summary>
    /// DTO for setting a score for an alternative on a criterion
    /// </summary>
    public class SetScoreDto
    {
        /// <summary>
        /// The alternative to score
        /// </summary>
        [Required]
        public Guid AlternativeId { get; set; }
        
        /// <summary>
        /// The criterion to score against
        /// </summary>
        [Required]
        public Guid CriterionId { get; set; }
        
        /// <summary>
        /// The score value (0-10)
        /// </summary>
        [Range(0, 10)]
        public decimal Score { get; set; }
        
        /// <summary>
        /// Justification for the score
        /// </summary>
        public string Justification { get; set; }
    }
}
