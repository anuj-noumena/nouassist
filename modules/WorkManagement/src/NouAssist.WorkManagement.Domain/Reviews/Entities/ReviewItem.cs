using System;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.Reviews.Entities
{
    public class ReviewItem : AuditedEntity<Guid>
    {
        public Guid ReviewId { get; private set; }
        public Guid EntityId { get; private set; }
        public ReviewItemType ItemType { get; private set; }
        public string ItemTitle { get; set; }
        public string ReviewNotes { get; set; }
        public bool IsReviewed { get; private set; }
        public DateTime? ReviewedAt { get; private set; }
        public ReviewItemDecision? Decision { get; private set; }
        public string ActionTaken { get; set; }

        protected ReviewItem() { /* ORM */ }

        public ReviewItem(Guid id, Guid reviewId, ReviewItemType itemType, Guid entityId, string itemTitle = null)
        {
            Id = id;
            ReviewId = reviewId;
            ItemType = itemType;
            EntityId = entityId;
            ItemTitle = itemTitle;
            IsReviewed = false;
        }

        public bool HasDecision => Decision.HasValue;
        public bool HasActionTaken => !string.IsNullOrWhiteSpace(ActionTaken);

        public void MarkAsReviewed(ReviewItemDecision decision, string notes = null, string actionTaken = null)
        {
            if (IsReviewed)
            {
                return;
            }

            IsReviewed = true;
            ReviewedAt = DateTime.UtcNow;
            Decision = decision;
            ReviewNotes = notes;
            ActionTaken = actionTaken;
        }

        public void AddNotes(string notes)
        {
            ReviewNotes = notes;
        }
    }
}
