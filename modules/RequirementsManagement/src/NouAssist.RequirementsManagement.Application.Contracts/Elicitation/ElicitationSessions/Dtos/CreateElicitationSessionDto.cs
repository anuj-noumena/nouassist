using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Enums;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos
{
    /// <summary>
    /// DTO for creating a new elicitation session
    /// </summary>
    public class CreateElicitationSessionDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxSessionTitleLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(ElicitationConsts.MaxSessionDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public SessionType Type { get; set; }

        [Required]
        public CreateSessionScheduleDto Schedule { get; set; } = new();

        [Required]
        public Guid OrganizerId { get; set; }

        public List<Guid> ParticipantIds { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating session schedule
    /// </summary>
    public class CreateSessionScheduleDto
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
