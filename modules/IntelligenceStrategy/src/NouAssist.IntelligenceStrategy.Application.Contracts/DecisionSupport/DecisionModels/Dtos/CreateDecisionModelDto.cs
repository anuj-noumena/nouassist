using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for creating a new decision model
    /// </summary>
    public class CreateDecisionModelDto
    {
        /// <summary>
        /// Title of the decision model
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxTitleLength)]
        public string Title { get; set; }
        
        /// <summary>
        /// Description of the decision context
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Type of decision model (e.g., "WeightedCriteria", "MAUT", "AHP")
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ModelType { get; set; }
    }
}
