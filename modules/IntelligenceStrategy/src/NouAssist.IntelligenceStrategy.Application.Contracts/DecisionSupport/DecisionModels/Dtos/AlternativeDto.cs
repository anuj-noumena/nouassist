using System;
using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for displaying alternative details
    /// </summary>
    public class AlternativeDto : EntityDto<Guid>
    {
        /// <summary>
        /// The decision model this alternative belongs to
        /// </summary>
        public Guid DecisionModelId { get; set; }
        
        /// <summary>
        /// Name of the alternative
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the alternative
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Scores for this alternative on different criteria
        /// </summary>
        public List<AlternativeScoreDto> Scores { get; set; }
        
        /// <summary>
        /// Whether this alternative was selected as the final decision
        /// </summary>
        public bool IsSelected { get; set; }
        
        /// <summary>
        /// Total weighted score (calculated from criteria weights and scores)
        /// </summary>
        public decimal? TotalScore { get; set; }
    }
}
