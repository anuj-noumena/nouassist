using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos
{
    /// <summary>
    /// DTO for predicting risk
    /// </summary>
    public class PredictRiskDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Target entity type
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TargetEntityType { get; set; }
        
        /// <summary>
        /// Target entity ID
        /// </summary>
        [Required]
        public Guid TargetEntityId { get; set; }
        
        /// <summary>
        /// Entity name
        /// </summary>
        [Required]
        [StringLength(PersonalProductivityIntelligenceConsts.MaxNameLength)]
        public string EntityName { get; set; }
        
        /// <summary>
        /// Entity description
        /// </summary>
        [StringLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength)]
        public string EntityDescription { get; set; }
    }
}
