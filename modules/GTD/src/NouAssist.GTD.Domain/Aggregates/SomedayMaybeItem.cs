using System;
using NouAssist.Core.Enums;
using NouAssist.Core.ValueObjects;
using NouAssist.GTD.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.GTD.Aggregates
{
    public class SomedayMaybeItem : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? Category { get; set; }
        public SomedayMaybeStatus Status { get; private set; }
        public Guid? ContextId { get; set; }
        public Guid? AreaId { get; set; }
        public string? Motivation { get; set; }
        public string? WhyNotNow { get; set; }
        public ImportanceScore Importance { get; private set; }
        public UrgencyScore Urgency { get; private set; }
        public string? PromotedToType { get; private set; }
        public Guid? PromotedToId { get; private set; }
        public DateTime? PromotedAt { get; private set; }
        public DateTime? LastReviewedAt { get; private set; }
        public int ReviewCount { get; private set; }
        public DateTime? ArchivedAt { get; private set; }

        protected SomedayMaybeItem() { }

        public SomedayMaybeItem(
            Guid id, 
            Guid ownerId, 
            string title,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            TenantId = tenantId;
            Status = SomedayMaybeStatus.Active;
            Importance = new ImportanceScore(0.40);
            Urgency = new UrgencyScore(0.10);
            ReviewCount = 0;
        }

        public void Promote(string promotedToType, Guid promotedToId)
        {
            if (Status == SomedayMaybeStatus.Promoted)
            {
                throw new BusinessException("GTD:ItemAlreadyPromoted", 
                    "Item is already promoted.");
            }

            Status = SomedayMaybeStatus.Promoted;
            PromotedToType = promotedToType;
            PromotedToId = promotedToId;
            PromotedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            Status = SomedayMaybeStatus.Archived;
            ArchivedAt = DateTime.UtcNow;
        }

        public void PutOnHold()
        {
            Status = SomedayMaybeStatus.OnHold;
        }

        public void Activate()
        {
            Status = SomedayMaybeStatus.Active;
        }

        public void Review()
        {
            LastReviewedAt = DateTime.UtcNow;
            ReviewCount++;
        }

        public void UpdateWhyNotNow(string reason)
        {
            WhyNotNow = reason;
        }

        public bool IsActive => Status == SomedayMaybeStatus.Active;
        public bool IsPromoted => Status == SomedayMaybeStatus.Promoted;
        public bool IsArchived => Status == SomedayMaybeStatus.Archived;
        public bool HasBeenReviewed => LastReviewedAt.HasValue;
        
        public TimeSpan? TimeSinceLastReview => 
            LastReviewedAt.HasValue ? DateTime.UtcNow - LastReviewedAt.Value : null;

        public int? DaysSinceLastReview => TimeSinceLastReview?.Days;
        
        public bool NeedsReview => 
            DaysSinceLastReview.HasValue && DaysSinceLastReview.Value > 90;

        public TimeSpan Age => DateTime.UtcNow - CreationTime;
        public int AgeInDays => Age.Days;
    }
}
