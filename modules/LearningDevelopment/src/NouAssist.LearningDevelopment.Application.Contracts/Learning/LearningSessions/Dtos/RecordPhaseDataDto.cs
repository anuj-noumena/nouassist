using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Learning.LearningSessions.Dtos
{
    public class RecordPreLearningQuestionsDto
    {
        [Required]
        [MinLength(1)]
        public List<string> Questions { get; set; }
    }

    public class RecordSectionReflectionDto
    {
        [Required]
        public string SectionKey { get; set; }

        [Required]
        public string Reflection { get; set; }
    }

    public class RecordFeynmanSummaryDto
    {
        [Required]
        [StringLength(4000, MinimumLength = 10)]
        public string Summary { get; set; }
    }

    public class RecordBloomAssessmentDto
    {
        [Required]
        [Range(0, 1)]
        public double RememberScore { get; set; }

        [Required]
        [Range(0, 1)]
        public double UnderstandScore { get; set; }

        [Required]
        [Range(0, 1)]
        public double ApplyScore { get; set; }

        [Required]
        [Range(0, 1)]
        public double AnalyzeScore { get; set; }

        [Required]
        [Range(0, 1)]
        public double EvaluateScore { get; set; }

        [Required]
        [Range(0, 1)]
        public double CreateScore { get; set; }
    }

    public class CompleteSpacedCycleDto
    {
        [Required]
        [Range(1, 4)]
        public int CycleNumber { get; set; }

        [Required]
        [Range(0, 1)]
        public double RecallAccuracy { get; set; }

        [Required]
        [Range(0, 1)]
        public double AverageConfidence { get; set; }

        [Required]
        [Range(0, 100)]
        public int CompletionRate { get; set; }

        public Dictionary<string, bool> Improvements { get; set; } = new();
        public Dictionary<string, string> Reflection { get; set; } = new();
    }

    public class RecordMasteryValidationDto
    {
        [Required]
        public Dictionary<string, object> ValidationData { get; set; }
    }
}
