using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;

namespace NouAssist.RequirementsManagement.Elicitation.Interviews.Dtos
{
    /// <summary>
    /// DTO for scheduling an interview
    /// </summary>
    public class ScheduleInterviewDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxSessionTitleLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(ElicitationConsts.MaxSessionDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [StringLength(ElicitationConsts.MaxLocationLength)]
        public string? Location { get; set; }

        [Required]
        public Guid OrganizerId { get; set; }

        [Required]
        public Guid IntervieweeId { get; set; }
    }
}
