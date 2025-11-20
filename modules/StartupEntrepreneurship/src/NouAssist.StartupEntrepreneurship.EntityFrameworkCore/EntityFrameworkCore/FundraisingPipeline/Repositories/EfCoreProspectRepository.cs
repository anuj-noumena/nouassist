using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Repositories
{
    public class EfCoreProspectRepository : 
        EfCoreRepository<IFundraisingPipelineDbContext, Prospect, Guid>, 
        IProspectRepository
    {
        public EfCoreProspectRepository(
            IDbContextProvider<IFundraisingPipelineDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Prospect> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Prospects
                .Include(p => p.Interactions)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Prospect>> GetByFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Prospects
                .Where(p => p.FundraisingRoundId == fundraisingRoundId)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Prospect>> GetByStatusAsync(
            ProspectStatus status, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // Since PipelineStatus is a value object, we need to query differently
            return await dbContext.Prospects
                .Where(p => EF.Property<string>(p, "Status") == status.ToString())
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountByStatusAsync(
            ProspectStatus status, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Prospects
                .Where(p => EF.Property<string>(p, "Status") == status.ToString())
                .CountAsync(cancellationToken);
        }

        public async Task<List<Prospect>> GetHighValueProspectsAsync(
            decimal minimumOpportunityValue = 1000000,
            CancellationToken cancellationToken = default)
        {
            var specification = new HighValueProspectSpecification(minimumOpportunityValue);
            return await GetBySpecificationAsync(specification, cancellationToken);
        }

        public async Task<List<Prospect>> GetProspectsNeedingFollowUpAsync(
            int maxDaysSinceLastInteraction = 14,
            CancellationToken cancellationToken = default)
        {
            var specification = new NeedsFollowUpProspectSpecification(maxDaysSinceLastInteraction);
            return await GetBySpecificationAsync(specification, cancellationToken);
        }

        public async Task<List<Prospect>> GetBySpecificationAsync(
            ProspectSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            query = query.Include(p => p.Interactions);
            
            return await query
                .Where(specification.ToExpression())
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Prospect>> WithDetailsAsync()
        {
            // Include Interactions by default when requesting details
            return (await GetQueryableAsync()).Include(p => p.Interactions);
        }
    }
}
