using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for simplified list view of decision models
    /// </summary>
    public class DecisionModelListDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Title of the decision model
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Type of decision model (e.g., "WeightedCriteria", "MAUT", "AHP")
        /// </summary>
        public string ModelType { get; set; }
        
        /// <summary>
        /// Whether the decision has been finalized
        /// </summary>
        public bool IsFinalized { get; set; }
        
        /// <summary>
        /// Date when the decision was finalized
        /// </summary>
        public DateTime? DecisionDate { get; set; }
        
        /// <summary>
        /// Number of criteria in the model
        /// </summary>
        public int CriteriaCount { get; set; }
        
        /// <summary>
        /// Number of alternatives in the model
        /// </summary>
        public int AlternativesCount { get; set; }
    }
}
