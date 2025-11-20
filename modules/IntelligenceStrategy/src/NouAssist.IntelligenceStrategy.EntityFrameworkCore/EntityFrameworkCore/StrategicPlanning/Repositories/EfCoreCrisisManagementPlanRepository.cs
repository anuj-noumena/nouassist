using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    /// EF Core repository implementation for Crisis Management Plan aggregate
    /// </summary>
    public class EfCoreCrisisManagementPlanRepository 
        : EfCoreRepository<IStrategicPlanningDbContext, CrisisManagementPlan, Guid>, ICrisisManagementPlanRepository 
    {
        public EfCoreCrisisManagementPlanRepository(
            IDbContextProvider<IStrategicPlanningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CrisisManagementPlan>> GetListAsync(
            Expression<Func<CrisisManagementPlan, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(predicate)
                .OrderBy(p => p.MaxCrisisLevel)
                .ThenBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<CrisisManagementPlan, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query.AnyAsync(predicate, cancellationToken);
        }

        public async Task<CrisisManagementPlan> GetActivePlanAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(p => p.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<CrisisManagementPlan>> GetPlansDueForReviewAsync(
            CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.UtcNow;
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.NextReviewDate <= currentDate)
                .OrderBy(p => p.NextReviewDate)
                .ToListAsync(cancellationToken);
        }

        // Override to include details by default
        public override async Task<CrisisManagementPlan> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                var dbContext = await GetDbContextAsync();
                query = dbContext.CrisisManagementPlans
                    .Include(p => p.ContingencyItems)
                    .Where(p => p.Id == id);
            }
            else
            {
                query = query.Where(p => p.Id == id);
            }

            var plan = await query.FirstOrDefaultAsync(cancellationToken);
            
            if (plan == null)
            {
                throw new EntityNotFoundException(typeof(CrisisManagementPlan), id);
            }

            return plan;
        }
    }
}
