using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a SWOT analysis and its strategic insights
    /// </summary>
    public class SwotAnalysis : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime AnalysisDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public bool IsCompleted { get; private set; }
        public string StrategicRecommendation { get; private set; }
        public string AnalysisNotes { get; private set; }

        // Navigation properties for entities within the aggregate
        private readonly List<SwotItem> _items;
        public IReadOnlyCollection<SwotItem> Items => _items.AsReadOnly();

        protected SwotAnalysis()
        {
            _items = new List<SwotItem>();
        }

        /// <summary>
        /// Creates a new SWOT analysis
        /// </summary>
        public static SwotAnalysis Create(
            Guid id,
            string title,
            string description,
            DateTime analysisDate,
            Guid? tenantId = null)
        {
            return new SwotAnalysis(id, title, description, analysisDate, tenantId);
        }

        private SwotAnalysis(
            Guid id,
            string title,
            string description,
            DateTime analysisDate,
            Guid? tenantId) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            AnalysisDate = analysisDate;
            TenantId = tenantId;
            IsCompleted = false;
            _items = new List<SwotItem>();
        }

        /// <summary>
        /// Updates analysis information
        /// </summary>
        public void UpdateAnalysis(string title, string description, string analysisNotes = null)
        {
            SetTitle(title);
            SetDescription(description);
            AnalysisNotes = analysisNotes?.Trim();

            // Validate notes length
            if (!string.IsNullOrEmpty(AnalysisNotes) && AnalysisNotes.Length > MarketIntelligenceConsts.Validation.MaxNotesLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Analysis Notes")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxNotesLength);
            }
        }

        /// <summary>
        /// Adds a SWOT item to the analysis
        /// </summary>
        public void AddItem(
            SwotCategory category,
            string title,
            string description,
            decimal impact = 0.5m,
            int priority = 1,
            string actionItems = null,
            string tags = null)
        {
            var item = new SwotItem(
                GuidGenerator.Create(),
                Id,
                category,
                title,
                description,
                impact,
                priority,
                actionItems,
                tags
            );

            _items.Add(item);
        }

        /// <summary>
        /// Updates an existing SWOT item
        /// </summary>
        public void UpdateItem(
            Guid itemId,
            string title,
            string description,
            decimal impact,
            int priority,
            string actionItems = null,
            string tags = null)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            
            if (item == null)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:SwotItemNotFound")
                    .WithData("itemId", itemId);
            }

            item.UpdateItem(title, description, impact, priority, actionItems, tags);
        }

        /// <summary>
        /// Removes a SWOT item
        /// </summary>
        public void RemoveItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        /// <summary>
        /// Completes the SWOT analysis
        /// </summary>
        public void CompleteAnalysis(string strategicRecommendation)
        {
            if (IsCompleted)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:AnalysisAlreadyCompleted");
            }

            if (_items.Count == 0)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:CannotCompleteEmptyAnalysis");
            }

            // Validate that we have at least one item in each category
            var hasStrengths = _items.Any(i => i.Category == SwotCategory.Strength);
            var hasWeaknesses = _items.Any(i => i.Category == SwotCategory.Weakness);
            var hasOpportunities = _items.Any(i => i.Category == SwotCategory.Opportunity);
            var hasThreats = _items.Any(i => i.Category == SwotCategory.Threat);

            if (!hasStrengths || !hasWeaknesses || !hasOpportunities || !hasThreats)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:IncompleteSWOTCategories")
                    .WithData("missingCategories", GetMissingCategories(hasStrengths, hasWeaknesses, hasOpportunities, hasThreats));
            }

            StrategicRecommendation = Check.NotNullOrWhiteSpace(
                strategicRecommendation,
                nameof(strategicRecommendation),
                maxLength: MarketIntelligenceConsts.SwotAnalysis.MaxDescriptionLength
            );

            IsCompleted = true;
            CompletedDate = DateTime.UtcNow;

            // Publish domain event
            AddDistributedEvent(new SwotAnalysisCompletedEto
            {
                Id = Id,
                Title = Title,
                ItemsCount = _items.Count,
                CompletedDate = CompletedDate.Value,
                StrategicRecommendation = StrategicRecommendation,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Reopens a completed SWOT analysis
        /// </summary>
        public void ReopenAnalysis()
        {
            if (!IsCompleted)
            {
                return;
            }

            IsCompleted = false;
            CompletedDate = null;
            StrategicRecommendation = null;
        }

        /// <summary>
        /// Gets SWOT items by category
        /// </summary>
        public IReadOnlyList<SwotItem> GetItemsByCategory(SwotCategory category)
        {
            return _items
                .Where(i => i.Category == category)
                .OrderByDescending(i => i.Impact)
                .ThenBy(i => i.Priority)
                .ToList();
        }

        /// <summary>
        /// Gets top priority items across all categories
        /// </summary>
        public IReadOnlyList<SwotItem> GetTopPriorityItems(int count = 3)
        {
            return _items
                .OrderByDescending(i => i.Impact)
                .ThenBy(i => i.Priority)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Gets a summary of items by category
        /// </summary>
        public Dictionary<SwotCategory, int> GetItemCategorySummary()
        {
            return _items
                .GroupBy(i => i.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Gets a custom analysis balance score
        /// </summary>
        public decimal GetAnalysisBalanceScore()
        {
            var summary = GetItemCategorySummary();
            
            // Calculate how balanced the analysis is across categories
            var totalItems = _items.Count;
            if (totalItems == 0)
            {
                return 0;
            }

            var idealPerCategory = totalItems / 4.0m; // 4 SWOT categories
            decimal balanceScore = 0;

            foreach (var category in Enum.GetValues(typeof(SwotCategory)).Cast<SwotCategory>())
            {
                var categoryCount = summary.ContainsKey(category) ? summary[category] : 0;
                var categoryBalance = Math.Min(categoryCount / idealPerCategory, 1.0m);
                balanceScore += categoryBalance;
            }

            return balanceScore / 4.0m; // Normalize to 0-1 range
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: MarketIntelligenceConsts.SwotAnalysis.MaxTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: MarketIntelligenceConsts.SwotAnalysis.MaxDescriptionLength
            );
        }

        private string GetMissingCategories(bool hasStrengths, bool hasWeaknesses, bool hasOpportunities, bool hasThreats)
        {
            var missingCategories = new List<string>();
            
            if (!hasStrengths) missingCategories.Add("Strengths");
            if (!hasWeaknesses) missingCategories.Add("Weaknesses");
            if (!hasOpportunities) missingCategories.Add("Opportunities");
            if (!hasThreats) missingCategories.Add("Threats");
            
            return string.Join(", ", missingCategories);
        }
    }
}
