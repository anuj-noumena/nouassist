using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Reviews.Reviews.Dtos
{
    public class ReviewDto : FullAuditedEntityDto<Guid>
    {
        public ReviewType ReviewType { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        public string WhatWentWell { get; set; }
        public string WhatNeedsAttention { get; set; }
        public string Insights { get; set; }
        public string LessonsLearned { get; set; }
        public string NextPeriodFocus { get; set; }
        public List<ReviewActionItemDto> ActionItems { get; set; }
        public int? CompletenessScore { get; set; }
        public int? SatisfactionScore { get; set; }
        public int? AlignmentScore { get; set; }
        public int? EnergyLevel { get; set; }
        public int? TimeSpentMinutes { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? NextReviewDue { get; set; }

        // Computed properties
        public bool HasActionItems { get; set; }
        public bool HasScores { get; set; }
        public bool HasPeriod { get; set; }
        public bool HasReflections { get; set; }
        public int? PeriodDurationDays { get; set; }
        public int TotalActionItems { get; set; }
        public int CompletedActionItems { get; set; }
        public int OverdueActionItems { get; set; }
        public double ActionItemsCompletionRate { get; set; }
        public int TotalReviewItems { get; set; }
        public int ReviewedItems { get; set; }
        public int RemainingItems { get; set; }
        public int CalculatedCompletenessScore { get; set; }
        public ReviewQuality Quality { get; set; }
    }

    public class ReviewActionItemDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? DueBy { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsAssigned { get; set; }
        public bool HasDueDate { get; set; }
        public bool IsOverdue { get; set; }
    }
}
