using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.StrategicInitiative
{
    /// <summary>
    /// DTO for creating a strategic initiative
    /// </summary>
    public class CreateStrategicInitiativeDto
    {
        /// <summary>
        /// The title of the initiative
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        
        /// <summary>
        /// A detailed description of the initiative
        /// </summary>
        [Required]
        public string Description { get; set; }
        
        /// <summary>
        /// The expected outcomes or goals of the initiative
        /// </summary>
        [Required]
        public string ExpectedOutcomes { get; set; }
        
        /// <summary>
        /// The rationale or business case for the initiative
        /// </summary>
        [Required]
        public string Rationale { get; set; }
        
        /// <summary>
        /// ID of the decision model that led to this initiative, if any
        /// </summary>
        public Guid? DecisionModelId { get; set; }
    }
}
