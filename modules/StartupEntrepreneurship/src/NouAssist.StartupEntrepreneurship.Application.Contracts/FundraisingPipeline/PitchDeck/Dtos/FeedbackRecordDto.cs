using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class FeedbackRecordDto : EntityDto<Guid>
    {
        public Guid PitchDeckId { get; set; }
        public Guid PresentationId { get; set; }
        public string ProspectName { get; set; }
        public FeedbackDataDto Feedback { get; set; }
        public string Notes { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class FeedbackDataDto
    {
        public FeedbackSentiment Sentiment { get; set; }
        public string SentimentName => Sentiment.ToString();
        public int Rating { get; set; }
        public Dictionary<string, string> CategoryFeedback { get; set; }
        public List<string> StrengthPoints { get; set; }
        public List<string> ImprovementAreas { get; set; }
    }
}
