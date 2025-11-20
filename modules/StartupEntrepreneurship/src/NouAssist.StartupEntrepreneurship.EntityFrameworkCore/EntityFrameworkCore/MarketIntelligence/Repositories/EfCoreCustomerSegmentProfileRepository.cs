using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.EntityFrameworkCore.Repositories
{
    public class EfCoreCustomerSegmentProfileRepository : 
        EfCoreRepository<IMarketIntelligenceDbContext, CustomerSegmentProfile, Guid>, 
        ICustomerSegmentProfileRepository
    {
        public EfCoreCustomerSegmentProfileRepository(
            IDbContextProvider<IMarketIntelligenceDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<CustomerSegmentProfile> FindByNameAsync(
            string name, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Characteristics);
            }
            
            return await query
                .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        }

        public async Task<List<CustomerSegmentProfile>> GetByTypeAsync(
            SegmentType type, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Characteristics);
            }
            
            return await query
                .Where(s => s.Type == type)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerSegmentProfile>> GetActiveAsync(
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Characteristics);
            }
            
            return await query
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerSegmentProfile>> GetByDemographicCriteriaAsync(
            string ageRange = null, 
            string income = null, 
            string location = null, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Characteristics);
            }
            
            // Filter by demographics (which are in a ValueObject)
            query = query
                .WhereIf(
                    !string.IsNullOrEmpty(ageRange),
                    s => EF.Property<string>(s, "Demographics_AgeRange").Contains(ageRange))
                .WhereIf(
                    !string.IsNullOrEmpty(income),
                    s => EF.Property<string>(s, "Demographics_Income").Contains(income))
                .WhereIf(
                    !string.IsNullOrEmpty(location),
                    s => EF.Property<string>(s, "Demographics_Location").Contains(location));
            
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerSegmentProfile>> GetListAsync(
            ISpecification<CustomerSegmentProfile> specification, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                query = query.Include(s => s.Characteristics);
            }
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<CustomerSegmentProfile>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(s => s.Characteristics);
        }
    }
}
