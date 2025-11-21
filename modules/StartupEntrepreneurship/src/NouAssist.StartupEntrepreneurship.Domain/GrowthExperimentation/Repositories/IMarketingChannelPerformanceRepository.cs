using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories
{
    /// <summary>
    /// Repository interface for MarketingChannelPerformance aggregate
    /// </summary>
    public interface IMarketingChannelPerformanceRepository : IRepository<MarketingChannelPerformance, Guid>
    {
        /// <summary>
        /// Gets a channel with all metrics included
        /// </summary>
        Task<MarketingChannelPerformance> GetWithMetricsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all active channels
        /// </summary>
        Task<List<MarketingChannelPerformance>> GetActiveChannelsAsync(
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets channels by type
        /// </summary>
        Task<List<MarketingChannelPerformance>> GetByTypeAsync(
            MarketingChannelType type,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a specific metric for a channel
        /// </summary>
        Task<ChannelMetric> GetMetricAsync(
            Guid channelId,
            Guid metricId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets metrics by name across all channels
        /// </summary>
        Task<List<ChannelMetric>> GetMetricsByNameAsync(
            string metricName,
            CancellationToken cancellationToken = default);
    }
}
