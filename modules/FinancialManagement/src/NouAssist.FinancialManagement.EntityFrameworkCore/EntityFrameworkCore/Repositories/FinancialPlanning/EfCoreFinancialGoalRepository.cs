using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.FinancialPlanning
{
    public class EfCoreFinancialGoalRepository 
        : EfCoreRepository<IFinancialManagementDbContext, FinancialGoal, Guid>,
          IFinancialGoalRepository
    {
        public EfCoreFinancialGoalRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<FinancialGoal>> GetByUserAsync(
            Guid userId,
            GoalStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.Where(g => g.UserId == userId);

            if (status.HasValue)
            {
                query = query.Where(g => g.Status == status.Value);
            }

            return await query
                .OrderBy(g => g.TargetDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<FinancialGoal>> GetGoalsDueBeforeDateAsync(
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(g => g.TargetDate <= date && 
                           g.Status != GoalStatus.Achieved && 
                           g.Status != GoalStatus.Abandoned)
                .OrderBy(g => g.TargetDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
