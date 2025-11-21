using System;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.Elicitation.Entities
{
    /// <summary>
    /// Entity for individual survey responses
    /// </summary>
    public class SurveyResponse : Entity<Guid>
    {
        public Guid SurveyId { get; private set; }
        public Guid RespondentId { get; private set; }
        public ResponseData ResponseData { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastModifiedAt { get; private set; }

        protected SurveyResponse() { }

        internal SurveyResponse(Guid surveyId, Guid respondentId, ResponseData responseData)
        {
            Id = Guid.NewGuid();
            SurveyId = surveyId;
            RespondentId = respondentId;
            ResponseData = Check.NotNull(responseData, nameof(responseData));
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateResponse(ResponseData responseData)
        {
            ResponseData = Check.NotNull(responseData, nameof(responseData));
            LastModifiedAt = DateTime.UtcNow;
        }

        public bool IsComplete(int totalQuestions)
        {
            return ResponseData.Answers.Count >= totalQuestions;
        }

        public TimeSpan GetResponseTime()
        {
            return ResponseData.ResponseTime;
        }
    }
}
