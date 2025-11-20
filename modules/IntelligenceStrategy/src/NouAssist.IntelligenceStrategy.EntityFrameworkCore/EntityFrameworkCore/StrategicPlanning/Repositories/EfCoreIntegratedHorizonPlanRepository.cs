using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core repository implementation for Integrated Horizon Plan aggregate
    /// </summary>
    public class EfCoreIntegratedHorizonPlanRepository 
        : EfCoreRepository<IStrategicPlanningDbContext, IntegratedHorizonPlan, Guid>, IIntegratedHorizonPlanRepository 
    {
        public EfCoreIntegratedHorizonPlanRepository(
            IDbContextProvider<IStrategicPlanningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<IntegratedHorizonPlan>> GetActivePlansAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(p => p.IsActive)
                .OrderBy(p => p.PlanStartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IntegratedHorizonPlan> GetPlanWithTimelineEventsAsync(
            Guid planId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var plan = await dbContext.IntegratedHorizonPlans
                .Include(p => p.TimelineEvents)
                .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);

            if (plan == null)
            {
                throw new EntityNotFoundException(typeof(IntegratedHorizonPlan), planId);
            }

            return plan;
        }

        public async Task<List<IntegratedHorizonPlan>> GetPlansByHorizonAsync(
            PlanningHorizon horizon,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(p => p.PrimaryHorizon == horizon)
                .OrderBy(p => p.PlanStartDate)
                .ToListAsync(cancellationToken);
        }

        // Override to include details by default
        public override async Task<IntegratedHorizonPlan> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetPlanWithTimelineEventsAsync(id, cancellationToken);
            }

            return await base.GetAsync(id, false, cancellationToken);
        }
    }
}
