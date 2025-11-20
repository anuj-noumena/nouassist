using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for adding a criterion to a decision model
    /// </summary>
    public class AddCriterionDto
    {
        /// <summary>
        /// Name of the criterion
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the criterion
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Weight of this criterion in the decision (normalized between 0 and 1)
        /// </summary>
        [Range(0, 1)]
        public decimal Weight { get; set; }
    }
}
