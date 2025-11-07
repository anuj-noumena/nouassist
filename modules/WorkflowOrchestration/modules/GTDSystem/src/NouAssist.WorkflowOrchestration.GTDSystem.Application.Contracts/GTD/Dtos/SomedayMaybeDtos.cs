using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkflowOrchestration.GTDSystem.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkflowOrchestration.GTDSystem.Dtos
{
    public class SomedayMaybeItemDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? Category { get; set; }
        public SomedayMaybeStatus Status { get; set; }
        public Guid? ContextId { get; set; }
        public Guid? AreaId { get; set; }
        public string? Motivation { get; set; }
        public string? WhyNotNow { get; set; }
        public double ImportanceScore { get; set; }
        public double UrgencyScore { get; set; }
        public string? PromotedToType { get; set; }
        public Guid? PromotedToId { get; set; }
        public DateTime? PromotedAt { get; set; }
        public DateTime? LastReviewedAt { get; set; }
        public int ReviewCount { get; set; }
        public int AgeInDays { get; set; }
        public bool NeedsReview { get; set; }
    }

    public class CreateSomedayMaybeItemDto
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Motivation { get; set; }
        public string? Category { get; set; }
    }

    public class UpdateSomedayMaybeItemDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? WhyNotNow { get; set; }
    }

    public class PromoteSomedayMaybeDto
    {
        [Required]
        public string PromotedToType { get; set; } // "Task", "Project", "Goal"
        [Required]
        public Guid PromotedToId { get; set; }
    }

    public class ReviewSomedayMaybeDto
    {
        public string? Decision { get; set; }
        public int? InterestLevel { get; set; } // 1-5
        public bool? ShouldPromote { get; set; }
        public string? Notes { get; set; }
    }

    public class GetSomedayMaybeListDto : PagedAndSortedResultRequestDto
    {
        public SomedayMaybeStatus? Status { get; set; }
        public string? Category { get; set; }
        public bool? NeedsReview { get; set; }
    }
}
