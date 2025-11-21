using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a market trend and its analysis
    /// </summary>
    public class MarketTrend : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public TrendDescription TrendDescription { get; private set; }
        public TrendCategory Category { get; private set; }
        public ImpactAssessment ImpactAssessment { get; private set; }
        public DateTime IdentifiedDate { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public bool IsActive { get; private set; }
        public string Tags { get; private set; }

        // Navigation properties for entities within the aggregate
        private readonly List<TrendDataSource> _dataSources;
        public IReadOnlyCollection<TrendDataSource> DataSources => _dataSources.AsReadOnly();

        protected MarketTrend()
        {
            _dataSources = new List<TrendDataSource>();
        }

        /// <summary>
        /// Creates a new market trend
        /// </summary>
        public static MarketTrend Create(
            Guid id,
            TrendDescription trendDescription,
            TrendCategory category,
            ImpactAssessment impactAssessment,
            DateTime identifiedDate,
            DateTime? expiryDate = null,
            Guid? tenantId = null)
        {
            return new MarketTrend(id, trendDescription, category, impactAssessment, identifiedDate, expiryDate, tenantId);
        }

        private MarketTrend(
            Guid id,
            TrendDescription trendDescription,
            TrendCategory category,
            ImpactAssessment impactAssessment,
            DateTime identifiedDate,
            DateTime? expiryDate,
            Guid? tenantId) : base(id)
        {
            TrendDescription = trendDescription ?? throw new ArgumentNullException(nameof(trendDescription));
            Category = category;
            ImpactAssessment = impactAssessment ?? throw new ArgumentNullException(nameof(impactAssessment));
            IdentifiedDate = identifiedDate;
            ExpiryDate = expiryDate;
            TenantId = tenantId;
            IsActive = true;
            _dataSources = new List<TrendDataSource>();

            ValidateExpiryDate();

            // Publish domain event
            AddDistributedEvent(new MarketTrendIdentifiedEto
            {
                Id = Id,
                Title = TrendDescription.Title,
                Category = Category,
                ImpactLevel = ImpactAssessment.Level,
                ConfidenceScore = ImpactAssessment.ConfidenceScore,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the trend information
        /// </summary>
        public void UpdateTrend(TrendDescription trendDescription, ImpactAssessment impactAssessment, string tags = null)
        {
            TrendDescription = trendDescription ?? throw new ArgumentNullException(nameof(trendDescription));
            ImpactAssessment = impactAssessment ?? throw new ArgumentNullException(nameof(impactAssessment));
            Tags = tags?.Trim();

            if (!string.IsNullOrEmpty(Tags) && Tags.Length > MarketIntelligenceConsts.Validation.MaxTagsLength)
            {
                throw new BusinessException("MarketIntelligence:Validation:StringLength")
                    .WithData("propertyName", "Tags")
                    .WithData("min", 0)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxTagsLength);
            }
        }

        /// <summary>
        /// Adds a data source for this trend
        /// </summary>
        public void AddDataSource(
            string sourceName,
            string sourceUrl,
            string sourceType,
            decimal reliability,
            DateTime collectedAt,
            string dataQuality = null,
            string extractedData = null)
        {
            // Check if source already exists
            if (_dataSources.Any(ds => ds.SourceUrl.Equals(sourceUrl, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:DuplicateDataSource")
                    .WithData("sourceUrl", sourceUrl);
            }

            var dataSource = new TrendDataSource(
                GuidGenerator.Create(),
                Id,
                sourceName,
                sourceUrl,
                sourceType,
                reliability,
                collectedAt,
                dataQuality,
                extractedData
            );

            _dataSources.Add(dataSource);
        }

        /// <summary>
        /// Updates an existing data source
        /// </summary>
        public void UpdateDataSource(
            Guid dataSourceId,
            string sourceName,
            string sourceUrl,
            decimal reliability,
            string dataQuality = null)
        {
            var dataSource = _dataSources.FirstOrDefault(ds => ds.Id == dataSourceId);
            
            if (dataSource == null)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:DataSourceNotFound")
                    .WithData("dataSourceId", dataSourceId);
            }

            dataSource.UpdateSource(sourceName, sourceUrl, reliability, dataQuality);
        }

        /// <summary>
        /// Removes a data source
        /// </summary>
        public void RemoveDataSource(Guid dataSourceId)
        {
            var dataSource = _dataSources.FirstOrDefault(ds => ds.Id == dataSourceId);
            
            if (dataSource != null)
            {
                _dataSources.Remove(dataSource);
            }
        }

        /// <summary>
        /// Extends the trend's relevance period
        /// </summary>
        public void ExtendTrend(DateTime newExpiryDate)
        {
            if (newExpiryDate <= DateTime.UtcNow)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:InvalidExpiryDate")
                    .WithData("expiryDate", newExpiryDate);
            }

            ExpiryDate = newExpiryDate;
        }

        /// <summary>
        /// Marks the trend as inactive
        /// </summary>
        public void DeactivateTrend(string reason = null)
        {
            IsActive = false;
            
            // Optionally add the reason as a note or log entry
            if (!string.IsNullOrEmpty(reason))
            {
                Tags = string.IsNullOrEmpty(Tags) ? $"Deactivated: {reason}" : $"{Tags}; Deactivated: {reason}";
            }
        }

        /// <summary>
        /// Reactivates the trend
        /// </summary>
        public void ReactivateTrend()
        {
            if (IsExpired())
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:CannotReactivateExpiredTrend");
            }

            IsActive = true;
        }

        /// <summary>
        /// Checks if the trend has expired
        /// </summary>
        public bool IsExpired()
        {
            return ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the average reliability score from all data sources
        /// </summary>
        public decimal GetAverageReliability()
        {
            if (!_dataSources.Any())
                return 0;

            return _dataSources.Average(ds => ds.Reliability);
        }

        /// <summary>
        /// Gets data sources with high reliability (>0.7)
        /// </summary>
        public IReadOnlyList<TrendDataSource> GetReliableDataSources()
        {
            return _dataSources
                .Where(ds => ds.Reliability > 0.7m)
                .OrderByDescending(ds => ds.Reliability)
                .ToList();
        }

        /// <summary>
        /// Checks if trend should trigger an opportunity alert
        /// </summary>
        public bool ShouldTriggerOpportunityAlert()
        {
            return IsActive &&
                   !IsExpired() &&
                   ImpactAssessment.Level >= ImpactLevel.High &&
                   ImpactAssessment.ConfidenceScore >= 0.8m &&
                   GetAverageReliability() >= 0.7m;
        }

        private void ValidateExpiryDate()
        {
            if (ExpiryDate.HasValue && ExpiryDate.Value <= IdentifiedDate)
            {
                throw new BusinessException("MarketIntelligence:ErrorMessage:InvalidExpiryDate")
                    .WithData("identifiedDate", IdentifiedDate)
                    .WithData("expiryDate", ExpiryDate);
            }
        }
    }
}
