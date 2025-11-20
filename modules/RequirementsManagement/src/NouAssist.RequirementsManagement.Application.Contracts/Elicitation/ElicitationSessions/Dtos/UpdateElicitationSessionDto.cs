using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos
{
    /// <summary>
    /// DTO for updating an existing elicitation session
    /// </summary>
    public class UpdateElicitationSessionDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxSessionTitleLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(ElicitationConsts.MaxSessionDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public UpdateSessionScheduleDto Schedule { get; set; } = new();

        public List<Guid> ParticipantIds { get; set; } = new();
    }

    /// <summary>
    /// DTO for updating session schedule
    /// </summary>
    public class UpdateSessionScheduleDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [StringLength(ElicitationConsts.MaxLocationLength)]
        public string? Location { get; set; }

        [StringLength(50)]
        public string TimeZone { get; set; } = "UTC";
    }
}
