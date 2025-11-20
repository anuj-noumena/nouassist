using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Repositories
{
    public class EfCoreCompetitorRepository : 
        EfCoreRepository<IMarketIntelligenceDbContext, Competitor, Guid>, 
        ICompetitorRepository
    {
        public EfCoreCompetitorRepository(
            IDbContextProvider<IMarketIntelligenceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Competitor> FindByNameAsync(
            string name, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(p => p.UpdateLogs);
            }

            return await query
                .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string name, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(c => c.Name == name)
                .WhereIf(excludeId.HasValue, c => c.Id != excludeId.Value)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> ExistsByWebsiteAsync(
            string website, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Since Website is in a ValueObject, we need to use EF.Property
            return await query
                .Where(c => EF.Property<string>(c, "CompetitorInfo_Website") == website)
                .WhereIf(excludeId.HasValue, c => c.Id != excludeId.Value)
                .AnyAsync(cancellationToken);
        }

        public async Task<List<Competitor>> GetByThreatLevelAsync(
            CompetitorThreatLevel threatLevel, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(c => c.UpdateLogs);
            }
            
            return await query
                .Where(c => c.ThreatLevel == threatLevel)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Competitor>> GetWithRecentUpdatesAsync(
            int days = 30, 
            int maxResultCount = 10, 
            CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            
            var query = await GetQueryableAsync();
            
            // Include UpdateLogs to filter
            query = query.Include(c => c.UpdateLogs);
            
            return await query
                .Where(c => c.UpdateLogs.Any(u => u.UpdateDate >= cutoffDate))
                .OrderByDescending(c => c.UpdateLogs.Max(u => u.UpdateDate))
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Competitor>> GetByIndustryAsync(
            string industry, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(c => c.UpdateLogs);
            }
            
            // Since Industry is in a ValueObject, we need to use EF.Property
            return await query
                .Where(c => EF.Property<string>(c, "CompetitorInfo_Industry").Contains(industry))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Competitor>> GetListAsync(
            ISpecification<Competitor> specification,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(c => c.UpdateLogs);
            }
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Competitor>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(c => c.UpdateLogs);
        }
    }
}
