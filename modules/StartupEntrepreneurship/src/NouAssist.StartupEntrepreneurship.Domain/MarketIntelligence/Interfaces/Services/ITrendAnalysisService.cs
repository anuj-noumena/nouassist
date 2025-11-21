using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services
{
    /// <summary>
    /// Interface for market trend analysis domain service
    /// </summary>
    public interface ITrendAnalysisService
    {
        /// <summary>
        /// Creates a new market trend
        /// </summary>
        Task<MarketTrend> CreateMarketTrendAsync(
            TrendDescription trendDescription,
            TrendCategory category,
            ImpactAssessment impactAssessment,
            DateTime? expiryDate = null);

        /// <summary>
        /// Updates a market trend
        /// </summary>
        Task<MarketTrend> UpdateMarketTrendAsync(
            Guid trendId,
            TrendDescription trendDescription,
            ImpactAssessment impactAssessment,
            string tags = null);

        /// <summary>
        /// Adds a data source to a market trend
        /// </summary>
        Task AddTrendDataSourceAsync(
            Guid trendId,
            string sourceName,
            string sourceUrl,
            string sourceType,
            decimal reliability,
            string dataQuality = null,
            string extractedData = null);

        /// <summary>
        /// Identifies high-impact trends
        /// </summary>
        Task<List<MarketTrend>> GetHighImpactTrendsAsync();

        /// <summary>
        /// Evaluates potential market opportunities based on trends
        /// </summary>
        Task<List<OpportunityResult>> EvaluateMarketOpportunitiesAsync();

        /// <summary>
        /// Gets active trends by category
        /// </summary>
        Task<List<MarketTrend>> GetActiveTrendsByCategoryAsync(TrendCategory category);

        /// <summary>
        /// Deactivates expired trends
        /// </summary>
        Task DeactivateExpiredTrendsAsync();
    }
}
