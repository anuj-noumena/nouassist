using System.ComponentModel.DataAnnotations;

namespace NouAssist.ReflectionAndSupport.Reviews.Dtos
{
    public class UpdateReviewReflectionsDto
    {
        [StringLength(2000)]
        public string WhatWentWell { get; set; }

        [StringLength(2000)]
        public string WhatNeedsAttention { get; set; }

        [StringLength(2000)]
        public string Insights { get; set; }

        [StringLength(2000)]
        public string LessonsLearned { get; set; }

        [StringLength(2000)]
        public string NextPeriodFocus { get; set; }
    }

    public class SetReviewScoresDto
    {
        [Range(0, 100)]
        public int? CompletenessScore { get; set; }

        [Range(0, 100)]
        public int? SatisfactionScore { get; set; }

        [Range(0, 100)]
        public int? AlignmentScore { get; set; }

        [Range(1, 10)]
        public int? EnergyLevel { get; set; }
    }

    public class CompleteReviewDto
    {
        [Range(0, 100)]
        public int? CompletenessScore { get; set; }

        [Range(0, 100)]
        public int? SatisfactionScore { get; set; }

        [Range(0, 100)]
        public int? AlignmentScore { get; set; }
    }

    public class AddReviewActionItemDto
    {
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string AssignedTo { get; set; }

        public DateTime? DueBy { get; set; }
    }

    public class RecordTimeSpentDto
    {
        [Required]
        [Range(0, 1440)]
        public int Minutes { get; set; }
    }
}
