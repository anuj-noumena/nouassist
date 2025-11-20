using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.Alternative
{
    /// <summary>
    /// DTO for updating an alternative
    /// </summary>
    public class UpdateAlternativeDto
    {
        /// <summary>
        /// The name of the alternative
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the alternative
        /// </summary>
        [Required]
        public string Description { get; set; }
    }
}
