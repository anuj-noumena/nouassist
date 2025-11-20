using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class PitchEffectivenessDto
    {
        public Guid PitchDeckId { get; set; }
        public string PitchDeckTitle { get; set; }
        public int TotalPresentations { get; set; }
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<FeedbackSentiment, int> SentimentBreakdown { get; set; }
        public Dictionary<string, double> VersionEffectiveness { get; set; }
        public List<string> TopStrengths { get; set; }
        public List<string> TopImprovementAreas { get; set; }
    }
}
