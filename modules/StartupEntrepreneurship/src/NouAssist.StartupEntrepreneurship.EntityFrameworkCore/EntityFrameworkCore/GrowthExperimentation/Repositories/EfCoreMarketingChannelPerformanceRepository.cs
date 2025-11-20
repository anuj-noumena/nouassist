using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.EntityFrameworkCore.Repositories
{
    public class EfCoreMarketingChannelPerformanceRepository 
        : EfCoreRepository<GrowthExperimentationDbContext, MarketingChannelPerformance, Guid>, IMarketingChannelPerformanceRepository
    {
        public EfCoreMarketingChannelPerformanceRepository(
            IDbContextProvider<GrowthExperimentationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<MarketingChannelPerformance> GetWithMetricsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(c => c.Metrics)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<MarketingChannelPerformance>> GetActiveChannelsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(c => c.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MarketingChannelPerformance>> GetByTypeAsync(
            MarketingChannelType type,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(c => c.Type == type)
                .ToListAsync(cancellationToken);
        }

        public async Task<ChannelMetric> GetMetricAsync(
            Guid channelId,
            Guid metricId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Set<ChannelMetric>()
                .Where(m => m.ChannelId == channelId && m.Id == metricId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<ChannelMetric>> GetMetricsByNameAsync(
            string metricName,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Set<ChannelMetric>()
                .Where(m => m.Name == metricName)
                .ToListAsync(cancellationToken);
        }
    }
}
