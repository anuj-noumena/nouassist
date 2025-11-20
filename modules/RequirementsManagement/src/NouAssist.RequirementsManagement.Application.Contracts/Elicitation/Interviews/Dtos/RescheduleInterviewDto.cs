using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;

namespace NouAssist.RequirementsManagement.Elicitation.Interviews.Dtos
{
    /// <summary>
    /// DTO for rescheduling an interview
    /// </summary>
    public class RescheduleInterviewDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [StringLength(ElicitationConsts.MaxLocationLength)]
        public string? Location { get; set; }
    }
}
