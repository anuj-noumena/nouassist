using System;
using System.Collections.Generic;

namespace NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos
{
    /// <summary>
    /// DTO for survey response
    /// </summary>
    public class SurveyResponseDto
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public Guid RespondentId { get; set; }
        public string? RespondentName { get; set; }
        public ResponseDataDto ResponseData { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public TimeSpan ResponseTime { get; set; }
        public bool IsComplete { get; set; }
    }

    /// <summary>
    /// DTO for response data
    /// </summary>
    public class ResponseDataDto
    {
        public Dictionary<string, object> Answers { get; set; } = new();
        public DateTime StartedAt { get; set; }
        public DateTime SubmittedAt { get; set; }
        public TimeSpan ResponseTime { get; set; }
    }
}
