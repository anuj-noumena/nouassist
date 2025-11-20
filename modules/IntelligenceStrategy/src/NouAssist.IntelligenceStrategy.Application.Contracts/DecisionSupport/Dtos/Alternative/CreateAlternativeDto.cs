using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.Alternative
{
    /// <summary>
    /// DTO for creating an alternative
    /// </summary>
    public class CreateAlternativeDto
    {
        /// <summary>
        /// The decision model this alternative belongs to
        /// </summary>
        [Required]
        public Guid DecisionModelId { get; set; }
        
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
