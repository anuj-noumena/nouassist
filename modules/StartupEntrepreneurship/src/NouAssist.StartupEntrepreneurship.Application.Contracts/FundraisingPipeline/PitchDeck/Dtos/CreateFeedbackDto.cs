using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos
{
    public class CreateFeedbackDto
    {
        [Required]
        public Guid PresentationId { get; set; }

        [Required]
        public CreateFeedbackDataDto Feedback { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }
    }

    public class CreateFeedbackDataDto
    {
        [Required]
        public FeedbackSentiment Sentiment { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int Rating { get; set; }

        public Dictionary<string, string> CategoryFeedback { get; set; }

        public List<string> StrengthPoints { get; set; }

        public List<string> ImprovementAreas { get; set; }
    }
}
