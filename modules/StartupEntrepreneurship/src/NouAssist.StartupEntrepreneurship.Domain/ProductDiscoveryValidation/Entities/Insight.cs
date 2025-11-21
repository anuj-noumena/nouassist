using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities
{
    /// <summary>
    /// Entity representing an insight extracted from customer interaction
    /// </summary>
    public class Insight : Entity<Guid>
    {
        public Guid InterviewId { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public int Importance { get; private set; } // 1-5
        public List<FeedbackTag> Tags { get; private set; }
        public DateTime ExtractedAt { get; private set; }
        public string ExtractedBy { get; private set; }

        protected Insight() 
        { 
            Tags = new List<FeedbackTag>();
        }

        internal Insight(
            Guid id,
            Guid interviewId,
            string description,
            string category,
            int importance,
            DateTime extractedAt,
            string extractedBy) : base(id)
        {
            InterviewId = interviewId;
            SetDescription(description);
            SetCategory(category);
            SetImportance(importance);
            ExtractedAt = extractedAt;
            ExtractedBy = Check.NotNullOrWhiteSpace(extractedBy, nameof(extractedBy));
            Tags = new List<FeedbackTag>();
        }

        internal void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        internal void UpdateImportance(int importance)
        {
            SetImportance(importance);
        }

        internal void AddTag(FeedbackTag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (!Tags.Any(t => t.Name == tag.Name))
            {
                Tags.Add(tag);
            }
        }

        internal void RemoveTag(string tagName)
        {
            Tags.RemoveAll(t => t.Name == tagName);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxInsightDescriptionLength
            );
        }

        private void SetCategory(string category)
        {
            Category = Check.NotNullOrWhiteSpace(
                category,
                nameof(category),
                maxLength: 100
            );
        }

        private void SetImportance(int importance)
        {
            if (importance < 1 || importance > 5)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidRange", 
                    "Importance must be between 1 and 5");
            }

            Importance = importance;
        }
    }
}
