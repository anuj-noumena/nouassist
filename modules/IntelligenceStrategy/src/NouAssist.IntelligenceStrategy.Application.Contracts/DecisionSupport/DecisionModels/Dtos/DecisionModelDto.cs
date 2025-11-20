using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for displaying decision model details
    /// </summary>
    public class DecisionModelDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// Title of the decision model
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the decision context
        /// </summary>
        public string Description { get; set; }
        
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
        /// ID of the selected alternative, if a decision was made
        /// </summary>
        public Guid? SelectedAlternativeId { get; set; }
        
        /// <summary>
        /// Criteria used in this decision model
        /// </summary>
        public List<CriterionDto> Criteria { get; set; }
        
        /// <summary>
        /// Alternatives being evaluated in this decision model
        /// </summary>
        public List<AlternativeDto> Alternatives { get; set; }
    }
}
