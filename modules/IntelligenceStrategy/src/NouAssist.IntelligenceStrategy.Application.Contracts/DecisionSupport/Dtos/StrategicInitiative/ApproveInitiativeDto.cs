using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.DecisionSupport.Dtos.StrategicInitiative
{
    /// <summary>
    /// DTO for approving a strategic initiative
    /// </summary>
    public class ApproveInitiativeDto
    {
        /// <summary>
        /// The ID of the project created from this initiative
        /// </summary>
        [Required]
        public Guid ProjectId { get; set; }
    }
}
