using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
    /// EF Core repository implementation for Strategic Goal Definition aggregate
    /// </summary>
    public class EfCoreStrategicGoalRepository 
        : EfCoreRepository<IStrategicPlanningDbContext, StrategicGoalDefinition, Guid>, IStrategicGoalRepository 
    {
        public EfCoreStrategicGoalRepository(
            IDbContextProvider<IStrategicPlanningDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<StrategicGoalDefinition>> GetListAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(predicate)
                .OrderBy(g => g.Priority)
                .ThenBy(g => g.TargetDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query.CountAsync(predicate, cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query.AnyAsync(predicate, cancellationToken);
        }

        public async Task<StrategicGoalDefinition> FindAsync(
            Expression<Func<StrategicGoalDefinition, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<List<StrategicGoalDefinition>> GetGoalsByOwnerAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(g => g.OwnerId == ownerId)
                .OrderBy(g => g.Priority)
                .ThenBy(g => g.TargetDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<StrategicGoalDefinition>> GetGoalsByHorizonAsync(
            PlanningHorizon horizon,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(g => g.Horizon == horizon)
                .OrderBy(g => g.Priority)
                .ThenBy(g => g.TargetDate)
                .ToListAsync(cancellationToken);
        }

        // Override to include details by default
        public override async Task<StrategicGoalDefinition> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                // Include related entities using Include
                var dbContext = await GetDbContextAsync();
                query = dbContext.StrategicGoals
                    .Include(g => g.Targets)
                    .Where(g => g.Id == id);
            }
            else
            {
                query = query.Where(g => g.Id == id);
            }

            var goal = await query.FirstOrDefaultAsync(cancellationToken);
            
            if (goal == null)
            {
                throw new EntityNotFoundException(typeof(StrategicGoalDefinition), id);
            }

            return goal;
        }

        public override async Task<StrategicGoalDefinition> FindAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeDetails)
            {
                var dbContext = await GetDbContextAsync();
                query = dbContext.StrategicGoals
                    .Include(g => g.Targets)
                    .Where(g => g.Id == id);
            }
            else
            {
                query = query.Where(g => g.Id == id);
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
