using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos
{
    /// <summary>
    /// DTO for submitting a survey response
    /// </summary>
    public class SubmitSurveyResponseDto
    {
        [Required]
        public Guid SurveyId { get; set; }

        [Required]
        public Guid RespondentId { get; set; }

        [Required]
        public Dictionary<string, object> Answers { get; set; } = new();

        [Required]
        public DateTime StartedAt { get; set; }

        [Required]
        public DateTime SubmittedAt { get; set; }
    }
}
