using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.Reviews.Enums;
using NouAssist.WorkManagement.Reviews.Events;
using NouAssist.WorkManagement.Reviews.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.Reviews.Aggregates
{
    public class Review : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public ReviewType ReviewType { get; private set; }
        public DateTime? PeriodStart { get; private set; }
        public DateTime? PeriodEnd { get; private set; }
        public string WhatWentWell { get; set; }
        public string WhatNeedsAttention { get; set; }
        public string Insights { get; set; }
        public string LessonsLearned { get; set; }
        public string NextPeriodFocus { get; set; }
        public int? CompletenessScore { get; private set; }
        public int? SatisfactionScore { get; private set; }
        public int? AlignmentScore { get; private set; }
        public int? EnergyLevel { get; private set; }
        public int? TimeSpentMinutes { get; private set; }
        public bool IsComplete { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DateTime? NextReviewDue { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<ReviewActionItem> _actionItems = new();
        public IReadOnlyCollection<ReviewActionItem> ActionItems => _actionItems.AsReadOnly();

        private readonly List<ReviewItem> _items = new();
        public IReadOnlyCollection<ReviewItem> Items => _items.AsReadOnly();

        protected Review() { /* ORM */ }

        public Review(
            Guid id,
            Guid ownerId,
            ReviewType reviewType,
            DateTime? periodStart = null,
            DateTime? periodEnd = null,
            Guid? tenantId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            ReviewType = reviewType;
            PeriodStart = periodStart ?? CalculateDefaultPeriodStart(reviewType, DateTime.UtcNow);
            PeriodEnd = periodEnd ?? DateTime.UtcNow;
            TenantId = tenantId;
            IsComplete = false;

            ValidatePeriod();
        }

        // Computed Properties
        public bool HasActionItems => _actionItems.Any();
        public bool HasScores => CompletenessScore.HasValue || SatisfactionScore.HasValue || AlignmentScore.HasValue;
        public bool HasPeriod => PeriodStart.HasValue && PeriodEnd.HasValue;
        public bool HasReflections => !string.IsNullOrWhiteSpace(WhatWentWell) || 
                                     !string.IsNullOrWhiteSpace(WhatNeedsAttention) || 
                                     !string.IsNullOrWhiteSpace(Insights);

        public TimeSpan? PeriodDuration => HasPeriod ? PeriodEnd.Value - PeriodStart.Value : null;
        public int? PeriodDurationDays => PeriodDuration?.Days;

        public int TotalActionItems => _actionItems.Count;
        public int CompletedActionItems => _actionItems.Count(i => i.IsCompleted);
        public int OverdueActionItems => _actionItems.Count(i => i.IsOverdue);
        public double ActionItemsCompletionRate => TotalActionItems == 0 ? 0.0 : (double)CompletedActionItems / TotalActionItems;

        public int TotalReviewItems => _items.Count;
        public int ReviewedItems => _items.Count(i => i.IsReviewed);
        public int RemainingItems => TotalReviewItems - ReviewedItems;

        // Completeness Calculation
        public double CalculatedCompleteness
        {
            get
            {
                int completed = 0;
                int total = 0;

                if (!string.IsNullOrWhiteSpace(WhatWentWell)) completed++;
                total++;

                if (!string.IsNullOrWhiteSpace(WhatNeedsAttention)) completed++;
                total++;

                if (!string.IsNullOrWhiteSpace(Insights)) completed++;
                total++;

                if (CompletenessScore.HasValue) completed++;
                total++;

                if (SatisfactionScore.HasValue) completed++;
                total++;

                if (HasActionItems) completed++;
                total++;

                return total > 0 ? (double)completed / total : 0.0;
            }
        }

        public int CalculatedCompletenessScore => (int)Math.Round(CalculatedCompleteness * 100);

        public ReviewQuality Quality
        {
            get
            {
                var score = CompletenessScore ?? CalculatedCompletenessScore;
                if (score >= 90) return ReviewQuality.Excellent;
                if (score >= 75) return ReviewQuality.Good;
                if (score >= 60) return ReviewQuality.Adequate;
                if (score >= 40) return ReviewQuality.Minimal;
                return ReviewQuality.Incomplete;
            }
        }

        // State Transitions
        public void Complete(int? completenessScore = null, int? satisfactionScore = null, int? alignmentScore = null)
        {
            if (IsComplete)
            {
                throw new BusinessException("Reviews:AlreadyCompleted", "Review is already completed");
            }

            IsComplete = true;
            CompletedAt = DateTime.UtcNow;
            CompletenessScore = completenessScore ?? CompletenessScore;
            SatisfactionScore = satisfactionScore ?? SatisfactionScore;
            AlignmentScore = alignmentScore ?? AlignmentScore;
            NextReviewDue = CalculateNextReviewDue(ReviewType, CompletedAt.Value);

            AddDistributedEvent(new ReviewCompletedEto
            {
                ReviewId = Id,
                TenantId = TenantId,
                OccurredAt = CompletedAt.Value,
                CompletenessScore = CompletenessScore,
                ReviewType = ReviewType.ToString()
            });
        }

        // Reflections
        public void UpdateReflections(
            string whatWentWell = null,
            string whatNeedsAttention = null,
            string insights = null,
            string lessonsLearned = null
        )
        {
            WhatWentWell = whatWentWell ?? WhatWentWell;
            WhatNeedsAttention = whatNeedsAttention ?? WhatNeedsAttention;
            Insights = insights ?? Insights;
            LessonsLearned = lessonsLearned ?? LessonsLearned;
        }

        // Action Items
        public void AddActionItem(Guid actionItemId, string description, string assignedTo = null, DateTime? dueBy = null)
        {
            var item = new ReviewActionItem(
                actionItemId,
                description,
                assignedTo,
                dueBy
            );

            _actionItems.Add(item);
        }

        public void CompleteActionItem(int index)
        {
            if (index < 0 || index >= _actionItems.Count)
            {
                throw new BusinessException("Reviews:InvalidActionItemIndex", "Invalid action item index");
            }

            var item = _actionItems[index];
            var completed = item.Complete();
            _actionItems[index] = completed;
        }

        // Review Items
        public void AddReviewItem(Guid itemId, ReviewItemType itemType, Guid entityId, string itemTitle = null)
        {
            if (_items.Any(i => i.Id == itemId))
            {
                throw new BusinessException("Reviews:DuplicateReviewItem", "Review item already exists");
            }

            var item = new ReviewItem(itemId, Id, itemType, entityId, itemTitle);
            _items.Add(item);
        }

        public void ReviewItem(Guid itemId, ReviewItemDecision decision, string notes = null, string actionTaken = null)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                throw new BusinessException("Reviews:ItemNotFound", "Review item not found");
            }

            item.MarkAsReviewed(decision, notes, actionTaken);
        }

        // Scores
        public void SetScores(int? completeness = null, int? satisfaction = null, int? alignment = null, int? energy = null)
        {
            ValidateScore(completeness, nameof(completeness));
            ValidateScore(satisfaction, nameof(satisfaction));
            ValidateScore(alignment, nameof(alignment));
            ValidateScore(energy, nameof(energy));

            CompletenessScore = completeness ?? CompletenessScore;
            SatisfactionScore = satisfaction ?? SatisfactionScore;
            AlignmentScore = alignment ?? AlignmentScore;
            EnergyLevel = energy ?? EnergyLevel;
        }

        public void RecordTimeSpent(int minutes)
        {
            if (minutes < 0 || minutes > 1440)
            {
                throw new BusinessException("Reviews:InvalidTimeSpent", 
                    "Time spent must be between 0 and 1440 minutes");
            }

            TimeSpentMinutes = (TimeSpentMinutes ?? 0) + minutes;
        }

        // Private Helpers
        private void ValidatePeriod()
        {
            if (PeriodStart.HasValue && PeriodEnd.HasValue && PeriodStart.Value > PeriodEnd.Value)
            {
                throw new BusinessException("Reviews:InvalidPeriod", 
                    "Period start must be before period end");
            }
        }

        private void ValidateScore(int? score, string fieldName)
        {
            if (score.HasValue && (score.Value < 0 || score.Value > 100))
            {
                throw new BusinessException("Reviews:InvalidScore", 
                    $"{fieldName} must be between 0 and 100");
            }
        }

        private static DateTime CalculateDefaultPeriodStart(ReviewType type, DateTime end)
        {
            return type switch
            {
                ReviewType.Daily => end.AddDays(-1),
                ReviewType.Weekly => end.AddDays(-7),
                ReviewType.Monthly => end.AddDays(-30),
                ReviewType.Quarterly => end.AddDays(-90),
                ReviewType.Yearly => end.AddDays(-365),
                ReviewType.AdHoc => end.AddDays(-7),
                _ => end.AddDays(-7)
            };
        }

        private static DateTime CalculateNextReviewDue(ReviewType type, DateTime from)
        {
            return type switch
            {
                ReviewType.Daily => from.AddDays(1),
                ReviewType.Weekly => from.AddDays(7),
                ReviewType.Monthly => from.AddDays(30),
                ReviewType.Quarterly => from.AddDays(90),
                ReviewType.Yearly => from.AddDays(365),
                ReviewType.AdHoc => from.AddDays(7),
                _ => from.AddDays(7)
            };
        }
    }
}
