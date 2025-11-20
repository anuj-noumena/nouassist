using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for approving a strategic initiative
    /// </summary>
    public class ApproveInitiativeDto
    {
        /// <summary>
        /// Optional ID of the project to associate with this initiative
        /// </summary>
        public Guid? ProjectId { get; set; }
    }
}
