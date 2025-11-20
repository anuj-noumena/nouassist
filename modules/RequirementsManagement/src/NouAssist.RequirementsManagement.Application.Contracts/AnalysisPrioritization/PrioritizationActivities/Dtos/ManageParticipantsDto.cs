using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO for managing activity participants
    /// </summary>
    public class ManageParticipantsDto
    {
        [Required]
        public List<Guid> ParticipantIds { get; set; } = new();
    }
}
