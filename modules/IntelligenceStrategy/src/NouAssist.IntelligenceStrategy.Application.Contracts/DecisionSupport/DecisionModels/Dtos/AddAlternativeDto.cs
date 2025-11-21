using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for adding an alternative to a decision model
    /// </summary>
    public class AddAlternativeDto
    {
        /// <summary>
        /// Name of the alternative
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the alternative
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
