using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.DecisionModel
{
    /// <summary>
    /// DTO for updating an existing decision model
    /// </summary>
    public class UpdateDecisionModelDto
    {
        /// <summary>
        /// The name of the decision model
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// A description of the decision to be made
        /// </summary>
        [Required]
        public string Description { get; set; }
        
        /// <summary>
        /// The context or background information for the decision
        /// </summary>
        [Required]
        public string Context { get; set; }
        
        /// <summary>
        /// Date when the decision needs to be made
        /// </summary>
        public DateTime? DecisionDueDate { get; set; }
    }
}
