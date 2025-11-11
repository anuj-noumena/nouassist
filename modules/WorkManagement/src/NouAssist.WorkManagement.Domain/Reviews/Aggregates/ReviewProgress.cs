using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.Reviews.Aggregates
{
    public class ReviewProgress : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid ReviewId { get; private set; }
        public Guid OwnerId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int TotalItems { get; private set; }
        public int ReviewedItems { get; private set; }
        public int RemainingItems { get; private set; }
        public double CompletionPercentage { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly Dictionary<string, object> _metadata = new();
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        protected ReviewProgress() { /* ORM */ }

        public ReviewProgress(
            Guid id,
            Guid reviewId,
            Guid ownerId,
            int totalItems = 0,
            int reviewedItems = 0,
            Guid? tenantId = null
        ) : base(id)
        {
            ReviewId = reviewId;
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            TenantId = tenantId;
            Timestamp = DateTime.UtcNow;
            
            UpdateProgress(reviewedItems, totalItems);
        }

        // Status Properties
        public bool IsComplete => RemainingItems == 0 && TotalItems > 0;
        public bool IsInProgress => ReviewedItems > 0 && RemainingItems > 0;
        public bool NotStarted => ReviewedItems == 0;

        // Progress Updates
        public void UpdateProgress(int reviewed, int total)
        {
            if (reviewed < 0 || total < 0)
            {
                throw new BusinessException("Reviews:InvalidProgress", 
                    "Progress values cannot be negative");
            }

            if (reviewed > total)
            {
                throw new BusinessException("Reviews:InvalidProgress", 
                    "Reviewed items cannot exceed total items");
            }

            TotalItems = total;
            ReviewedItems = reviewed;
            RemainingItems = total - reviewed;
            CompletionPercentage = total > 0 ? (double)reviewed / total * 100 : 0.0;
            Timestamp = DateTime.UtcNow;
        }

        public void IncrementReviewed()
        {
            if (ReviewedItems >= TotalItems)
            {
                throw new BusinessException("Reviews:MaxItemsReviewed", 
                    "All items have been reviewed");
            }

            UpdateProgress(ReviewedItems + 1, TotalItems);
        }

        public void DecrementReviewed()
        {
            if (ReviewedItems <= 0)
            {
                throw new BusinessException("Reviews:MinItemsReviewed", 
                    "Cannot decrement below zero");
            }

            UpdateProgress(ReviewedItems - 1, TotalItems);
        }

        public void SetTotalItems(int total)
        {
            UpdateProgress(ReviewedItems, total);
        }

        // Metadata Management
        public void SetMetadata(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new BusinessException("Reviews:InvalidMetadataKey", 
                    "Metadata key cannot be empty");
            }

            _metadata[key] = value;
        }

        public void RemoveMetadata(string key)
        {
            _metadata.Remove(key);
        }

        public void ClearMetadata()
        {
            _metadata.Clear();
        }
    }
}
