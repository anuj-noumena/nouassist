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
    /// EF Core repository implementation for Transition Plan aggregate
    /// </summary>
    public class EfCoreTransitionPlanRepository 
        : EfCoreRepository<IStrategicPlanningDbContext, TransitionPlan, Guid>, ITransitionPlanRepository 
    {
        public EfCoreTransitionPlanRepository(
            IDbContextProvider<IStrategicPlanningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<TransitionPlan>> GetActivePlansAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(p => p.IsActive)
                .OrderBy(p => p.PlannedStartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<TransitionPlan> GetPlanWithCapabilityRoadmapAsync(
            Guid planId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var plan = await dbContext.TransitionPlans
                .Include(p => p.CapabilityRoadmap)
                .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);

            if (plan == null)
            {
                throw new EntityNotFoundException(typeof(TransitionPlan), planId);
            }

            return plan;
        }

        public async Task<List<TransitionPlan>> GetPlansByTypeAsync(
            TransitionType type,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(p => p.Type == type)
                .OrderBy(p => p.PlannedStartDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TransitionPlan>> GetPlansRequiringAttentionAsync(
            CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.UtcNow;
            var query = await GetQueryableAsync();
            
            return await query
                .Where(p => p.IsActive && 
                           (p.EstimatedCompletionDate < currentDate || 
                            p.CurrentPhase == "Paused" ||
                            p.CurrentPhase == "At Risk"))
                .OrderBy(p => p.EstimatedCompletionDate)
                .ToListAsync(cancellationToken);
        }

        // Override to include details by default
        public override async Task<TransitionPlan> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetPlanWithCapabilityRoadmapAsync(id, cancellationToken);
            }

            return await base.GetAsync(id, false, cancellationToken);
        }
    }
}
