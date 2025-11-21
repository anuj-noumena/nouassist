using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Repositories
{
    public class EfCoreMarketTrendRepository : 
        EfCoreRepository<IMarketIntelligenceDbContext, MarketTrend, Guid>, 
        IMarketTrendRepository
    {
        public EfCoreMarketTrendRepository(
            IDbContextProvider<IMarketIntelligenceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<MarketTrend>> GetActiveAsync(
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(t => t.DataSources);
            }
            
            return await query
                .Where(t => t.IsActive && (!t.ExpiryDate.HasValue || t.ExpiryDate.Value > DateTime.UtcNow))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MarketTrend>> GetByCategoryAsync(
            TrendCategory category, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(t => t.DataSources);
            }
            
            return await query
                .Where(t => t.Category == category)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MarketTrend>> GetByImpactLevelAsync(
            ImpactLevel impactLevel, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(t => t.DataSources);
            }
            
            // Since ImpactLevel is in a ValueObject, we need to use EF.Property
            return await query
                .Where(t => EF.Property<string>(t, "ImpactAssessment_Level") == impactLevel.ToString())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MarketTrend>> GetByKeywordsAsync(
            string keywords, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(t => t.DataSources);
            }
            
            // Since Keywords is in a ValueObject, we need to use EF.Property
            return await query
                .Where(t => 
                    EF.Property<string>(t, "TrendDescription_Keywords").Contains(keywords) ||
                    EF.Property<string>(t, "TrendDescription_Title").Contains(keywords) ||
                    EF.Property<string>(t, "TrendDescription_Description").Contains(keywords))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MarketTrend>> GetHighConfidenceTrendsAsync(
            decimal minConfidenceScore = 0.8m, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(t => t.DataSources);
            }
            
            // Since ConfidenceScore is in a ValueObject, we need to use EF.Property
            return await query
                .Where(t => EF.Property<decimal>(t, "ImpactAssessment_ConfidenceScore") >= minConfidenceScore)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MarketTrend>> GetListAsync(
            ISpecification<MarketTrend> specification, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(t => t.DataSources);
            }
            
            return await DynamicQueryableExtensions.Where(query, specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<MarketTrend>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(t => t.DataSources);
        }
    }
}
