using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for updating an existing strategic initiative
    /// </summary>
    public class UpdateStrategicInitiativeDto
    {
        /// <summary>
        /// Title of the strategic initiative
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxTitleLength)]
        public string Title { get; set; }
        
        /// <summary>
        /// Detailed description of the initiative
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// The rationale behind this strategic initiative
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxRationaleLength)]
        public string Rationale { get; set; }
        
        /// <summary>
        /// Expected outcomes from implementing this initiative
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxExpectedOutcomesLength)]
        public string ExpectedOutcomes { get; set; }
        
        /// <summary>
        /// ID of any related decision model that informed this initiative
        /// </summary>
        public Guid? RelatedDecisionModelId { get; set; }
        
        /// <summary>
        /// The degree to which this initiative aligns with strategic goals
        /// </summary>
        public AlignmentStatus StrategicAlignment { get; set; }
    }
}
