using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend
{
    /// <summary>
    /// Interface for market trend management application service
    /// </summary>
    public interface IMarketTrendAppService : IApplicationService
    {
        /// <summary>
        /// Gets a market trend by ID
        /// </summary>
        Task<MarketTrendDto> GetAsync(Guid id);

        /// <summary>
        /// Gets a paged list of market trends with optional filtering
        /// </summary>
        Task<PagedResultDto<MarketTrendDto>> GetListAsync(GetMarketTrendListDto input);

        /// <summary>
        /// Creates a new market trend
        /// </summary>
        Task<MarketTrendDto> CreateAsync(CreateMarketTrendDto input);

        /// <summary>
        /// Updates an existing market trend
        /// </summary>
        Task<MarketTrendDto> UpdateAsync(Guid id, UpdateMarketTrendDto input);

        /// <summary>
        /// Deletes a market trend
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Adds a data source to a market trend
        /// </summary>
        Task<TrendDataSourceDto> AddDataSourceAsync(Guid id, CreateTrendDataSourceDto input);

        /// <summary>
        /// Removes a data source from a market trend
        /// </summary>
        Task DeleteDataSourceAsync(Guid trendId, Guid dataSourceId);

        /// <summary>
        /// Deactivates a market trend
        /// </summary>
        Task<MarketTrendDto> DeactivateAsync(Guid id, string reason = null);

        /// <summary>
        /// Reactivates a market trend
        /// </summary>
        Task<MarketTrendDto> ReactivateAsync(Guid id);

        /// <summary>
        /// Extends the expiry date of a market trend
        /// </summary>
        Task<MarketTrendDto> ExtendExpiryDateAsync(Guid id, DateTime newExpiryDate);

        /// <summary>
        /// Gets active market trends by category
        /// </summary>
        Task<List<MarketTrendDto>> GetActiveByCategoryAsync(TrendCategory category);

        /// <summary>
        /// Gets high-impact market trends
        /// </summary>
        Task<List<MarketTrendDto>> GetHighImpactTrendsAsync();

        /// <summary>
        /// Identifies market opportunities based on trends
        /// </summary>
        Task<List<OpportunityAlertDto>> GetMarketOpportunitiesAsync();
    }
}
