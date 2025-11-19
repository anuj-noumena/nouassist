using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Repositories
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

        public async Task<List<FinancialGoal>> GetGoalsByUserAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.FinancialGoals
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.CreationTime)
                .ToListAsync();
        }

        public async Task<List<FinancialGoal>> GetActiveGoalsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.FinancialGoals
                .Where(g => g.UserId == userId && g.Status == GoalStatus.Active)
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
        }

        public async Task<List<FinancialGoal>> GetGoalsByTypeAsync(Guid userId, GoalType type)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.FinancialGoals
                .Where(g => g.UserId == userId && g.Type == type)
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
        }

        public async Task<List<FinancialGoal>> GetGoalsByStatusAsync(Guid userId, GoalStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.FinancialGoals
                .Where(g => g.UserId == userId && g.Status == status)
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
        }

        public async Task<List<FinancialGoal>> GetGoalsDueByDateAsync(Guid userId, DateTime targetDate)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.FinancialGoals
                .Where(g => g.UserId == userId
                    && g.TargetDate <= targetDate
                    && g.Status == GoalStatus.Active)
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
        }
    }
}
