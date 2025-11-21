using System;
using NouAssist.RequirementsManagement.Elicitation.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.Elicitation.Events
{
    /// <summary>
    /// Event published when a survey response is submitted
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Elicitation.SurveyResponseSubmitted")]
    public class SurveyResponseSubmittedEto : ElicitationEtoBase
    {
        public Guid SurveyId { get; set; }
        public Guid RespondentId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int QuestionsAnswered { get; set; }
    }
}
