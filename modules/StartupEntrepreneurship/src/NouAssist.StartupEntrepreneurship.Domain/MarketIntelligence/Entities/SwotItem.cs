using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities
{
    /// <summary>
    /// Represents a single item in a SWOT analysis
    /// </summary>
    public class SwotItem : Entity<Guid>
    {
        public Guid SwotAnalysisId { get; private set; }
        public SwotCategory Category { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Impact { get; private set; }
        public string ActionItems { get; private set; }
        public string Tags { get; private set; }
        public int Priority { get; private set; }

        protected SwotItem() { }

        internal SwotItem(
            Guid id,
            Guid swotAnalysisId,
            SwotCategory category,
            string title,
            string description,
            decimal impact = 0.5m,
            int priority = 1,
            string actionItems = null,
            string tags = null) : base(id)
        {
            SwotAnalysisId = swotAnalysisId;
            Category = category;
            SetTitle(title);
            SetDescription(description);
            SetImpact(impact);
            SetPriority(priority);
            ActionItems = actionItems?.Trim();
            Tags = tags?.Trim();

            ValidateCategory();
        }

        internal void UpdateItem(
            string title,
            string description,
            decimal impact,
            int priority,
            string actionItems = null,
            string tags = null)
        {
            SetTitle(title);
            SetDescription(description);
            SetImpact(impact);
            SetPriority(priority);
            ActionItems = actionItems?.Trim();
            Tags = tags?.Trim();
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: MarketIntelligenceConsts.SwotAnalysis.MaxItemTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: MarketIntelligenceConsts.SwotAnalysis.MaxItemDescriptionLength
            );
        }

        private void SetImpact(decimal impact)
        {
            if (impact < MarketIntelligenceConsts.Validation.MinConfidenceScore ||
                impact > MarketIntelligenceConsts.Validation.MaxConfidenceScore)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidScore")
                    .WithData("min", MarketIntelligenceConsts.Validation.MinConfidenceScore)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxConfidenceScore);
            }

            Impact = impact;
        }

        private void SetPriority(int priority)
        {
            if (priority < 1 || priority > 10)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidValue")
                    .WithData("propertyName", "Priority")
                    .WithData("allowedRange", "1-10");
            }

            Priority = priority;
        }

        private void ValidateCategory()
        {
            if (!Enum.IsDefined(typeof(SwotCategory), Category))
            {
                throw new InvalidSwotItemException(Category, "Invalid SWOT category");
            }
        }

        private void ValidateActionItems()
        {
            if (!string.IsNullOrEmpty(ActionItems) && ActionItems.Length > MarketIntelligenceConsts.SwotAnalysis.MaxActionItemLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Action Items")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.SwotAnalysis.MaxActionItemLength);
            }
        }
    }
}
