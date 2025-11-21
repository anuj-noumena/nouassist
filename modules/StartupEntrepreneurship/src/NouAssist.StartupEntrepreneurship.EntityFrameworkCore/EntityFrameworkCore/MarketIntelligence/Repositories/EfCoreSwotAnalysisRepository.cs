using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Repositories
{
    public class EfCoreSwotAnalysisRepository : 
        EfCoreRepository<IMarketIntelligenceDbContext, SwotAnalysis, Guid>, 
        ISwotAnalysisRepository
    {
        public EfCoreSwotAnalysisRepository(
            IDbContextProvider<IMarketIntelligenceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SwotAnalysis>> GetCompletedAsync(
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Items);
            }
            
            return await query
                .Where(s => s.IsCompleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SwotAnalysis>> GetIncompleteAsync(
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Items);
            }
            
            return await query
                .Where(s => !s.IsCompleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<SwotAnalysis> FindByTitleAsync(
            string title, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Items);
            }
            
            return await query
                .FirstOrDefaultAsync(s => s.Title == title, cancellationToken);
        }

        public async Task<List<SwotAnalysis>> GetRecentAsync(
            int maxResultCount = 10, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Items);
            }
            
            return await query
                .OrderByDescending(s => s.AnalysisDate)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SwotAnalysis>> GetListAsync(
            ISpecification<SwotAnalysis> specification, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Items);
            }
            
            return await DynamicQueryableExtensions.Where(query, specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<SwotAnalysis>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(s => s.Items);
        }
    }
}
