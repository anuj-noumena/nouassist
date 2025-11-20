using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos
{
    /// <summary>
    /// DTO for finalizing a decision model
    /// </summary>
    public class FinalizeDecisionDto
    {
        /// <summary>
        /// The selected alternative ID
        /// </summary>
        [Required]
        public Guid SelectedAlternativeId { get; set; }
    }
}
