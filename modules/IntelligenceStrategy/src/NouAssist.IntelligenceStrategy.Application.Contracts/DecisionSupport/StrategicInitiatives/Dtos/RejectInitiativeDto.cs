using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for rejecting a strategic initiative
    /// </summary>
    public class RejectInitiativeDto
    {
        /// <summary>
        /// Reason for rejecting the initiative
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxDescriptionLength)]
        public string RejectionReason { get; set; }
    }
}
