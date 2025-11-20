using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.StrategicInitiative
{
    /// <summary>
    /// DTO for rejecting a strategic initiative
    /// </summary>
    public class RejectInitiativeDto
    {
        /// <summary>
        /// The reason for rejection
        /// </summary>
        [Required]
        public string Reason { get; set; }
    }
}
