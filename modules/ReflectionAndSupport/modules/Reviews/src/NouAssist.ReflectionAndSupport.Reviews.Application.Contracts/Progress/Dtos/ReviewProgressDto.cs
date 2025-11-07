using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ReflectionAndSupport.Reviews.Progress.Dtos
{
    public class ReviewProgressDto : EntityDto<Guid>
    {
        public Guid ReviewId { get; set; }
        public DateTime Timestamp { get; set; }
        public int TotalItems { get; set; }
        public int ReviewedItems { get; set; }
        public int RemainingItems { get; set; }
        public double CompletionPercentage { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public bool IsComplete { get; set; }
        public bool IsInProgress { get; set; }
        public bool NotStarted { get; set; }
    }

    public class ReviewQualityAnalysisDto
    {
        public bool HasReflections { get; set; }
        public bool HasScores { get; set; }
        public bool HasActionItems { get; set; }
        public int CompletenessScore { get; set; }
        public int ReflectionDepth { get; set; }
        public int ActionItemQuality { get; set; }
        public ReviewQuality OverallQuality { get; set; }
        public int QualityScore { get; set; }
        public List<string> Recommendations { get; set; }
    }
}
