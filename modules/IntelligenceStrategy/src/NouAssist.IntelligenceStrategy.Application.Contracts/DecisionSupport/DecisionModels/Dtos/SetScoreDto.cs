using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for setting a score for an alternative on a criterion
    /// </summary>
    public class SetScoreDto
    {
        /// <summary>
        /// The criterion being scored against
        /// </summary>
        [Required]
        public Guid CriterionId { get; set; }
        
        /// <summary>
        /// Score value (normalized between 0 and 1)
        /// </summary>
        [Range(0, 1)]
        public decimal Score { get; set; }
    }
}
