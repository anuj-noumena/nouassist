using System;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.ValidationVerification.Entities
{
    /// <summary>
    /// Represents feedback provided by a reviewer for a requirement review
    /// </summary>
    public class ReviewFeedbackItem : Entity<Guid>
    {
        public Guid ReviewCycleId { get; private set; }
        public Guid ReviewerId { get; private set; }
        public ReviewDecision Decision { get; private set; }
        public FeedbackComment Comment { get; private set; }
        public DateTime SubmittedAt { get; private set; }
        public string ReviewerName { get; private set; }

        protected ReviewFeedbackItem() { }

        internal static ReviewFeedbackItem Create(
            Guid id,
            Guid reviewCycleId,
            Guid reviewerId,
            ReviewDecision decision,
            FeedbackComment comment,
            DateTime submitTime)
        {
            return new ReviewFeedbackItem(id, reviewCycleId, reviewerId, decision, comment, submitTime);
        }

        private ReviewFeedbackItem(
            Guid id,
            Guid reviewCycleId,
            Guid reviewerId,
            ReviewDecision decision,
            FeedbackComment comment,
            DateTime submitTime) : base(id)
        {
            ReviewCycleId = reviewCycleId;
            ReviewerId = reviewerId;
            Decision = Check.NotNull(decision, nameof(decision));
            Comment = comment; // Optional - can be null
            SubmittedAt = submitTime;
        }

        internal void UpdateDecision(ReviewDecision newDecision, DateTime updateTime)
        {
            Decision = Check.NotNull(newDecision, nameof(newDecision));
        }

        internal void AddComment(FeedbackComment comment)
        {
            Comment = Check.NotNull(comment, nameof(comment));
        }

        public bool IsPositiveDecision()
        {
            return Decision.IsPositive();
        }

        public bool IsNegativeDecision()
        {
            return Decision.IsNegative();
        }

        public bool RequiresChanges()
        {
            return Decision.RequiresChanges();
        }
    }
}
