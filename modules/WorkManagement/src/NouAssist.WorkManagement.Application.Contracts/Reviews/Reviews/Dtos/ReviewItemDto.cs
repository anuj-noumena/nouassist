using System;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Reviews.Reviews.Dtos
{
    public class ReviewItemDto : AuditedEntityDto<Guid>
    {
        public Guid ReviewId { get; set; }
        public Guid EntityId { get; set; }
        public ReviewItemType ItemType { get; set; }
        public string ItemTitle { get; set; }
        public string ReviewNotes { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public ReviewItemDecision? Decision { get; set; }
        public string ActionTaken { get; set; }
        public bool HasDecision { get; set; }
        public bool HasActionTaken { get; set; }
    }

    public class AddReviewItemDto
    {
        [Required]
        public ReviewItemType ItemType { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        [StringLength(200)]
        public string ItemTitle { get; set; }
    }

    public class ReviewItemDecisionDto
    {
        [Required]
        public ReviewItemDecision Decision { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        [StringLength(1000)]
        public string ActionTaken { get; set; }
    }
}
