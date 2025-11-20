using System;
using System.Collections.Generic;

namespace NouAssist.RequirementsManagement.Elicitation.Surveys.Dtos
{
    /// <summary>
    /// DTO for survey analytics
    /// </summary>
    public class SurveyAnalyticsDto
    {
        public Guid SurveyId { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalResponses { get; set; }
        public double CompletionRate { get; set; }
        public TimeSpan AverageResponseTime { get; set; }
        public Dictionary<DateTime, int> ResponsesByDate { get; set; } = new();
        public int IncompleteResponses { get; set; }
        public int CompletedResponses { get; set; }
    }
}
