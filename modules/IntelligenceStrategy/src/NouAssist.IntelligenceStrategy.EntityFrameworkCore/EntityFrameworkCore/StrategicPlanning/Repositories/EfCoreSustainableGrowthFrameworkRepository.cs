using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core repository implementation for Sustainable Growth Framework aggregate
    /// </summary>
    public class EfCoreSustainableGrowthFrameworkRepository 
        : EfCoreRepository<IStrategicPlanningDbContext, SustainableGrowthFramework, Guid>, ISustainableGrowthFrameworkRepository 
    {
        public EfCoreSustainableGrowthFrameworkRepository(
            IDbContextProvider<IStrategicPlanningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SustainableGrowthFramework>> GetActiveFrameworksAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(f => f.IsActive)
                .OrderBy(f => f.FrameworkStartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<SustainableGrowthFramework> GetFrameworkWithEsgGoalsAsync(
            Guid frameworkId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var framework = await dbContext.SustainableGrowthFrameworks
                .Include(f => f.EsgGoals)
                .FirstOrDefaultAsync(f => f.Id == frameworkId, cancellationToken);

            if (framework == null)
            {
                throw new EntityNotFoundException(typeof(SustainableGrowthFramework), frameworkId);
            }

            return framework;
        }

        // Override to include details by default
        public override async Task<SustainableGrowthFramework> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetFrameworkWithEsgGoalsAsync(id, cancellationToken);
            }

            return await base.GetAsync(id, false, cancellationToken);
        }
    }
}
