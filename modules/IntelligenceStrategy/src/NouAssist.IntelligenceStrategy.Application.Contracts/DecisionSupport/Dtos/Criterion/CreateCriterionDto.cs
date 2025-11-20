using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.Criterion
{
    /// <summary>
    /// DTO for creating a criterion
    /// </summary>
    public class CreateCriterionDto
    {
        /// <summary>
        /// The decision model this criterion belongs to
        /// </summary>
        [Required]
        public Guid DecisionModelId { get; set; }
        
        /// <summary>
        /// The name of the criterion
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the criterion
        /// </summary>
        [Required]
        public string Description { get; set; }
        
        /// <summary>
        /// The weight of the criterion (0-1)
        /// </summary>
        [Range(0, 1)]
        public decimal Weight { get; set; }
    }
}
