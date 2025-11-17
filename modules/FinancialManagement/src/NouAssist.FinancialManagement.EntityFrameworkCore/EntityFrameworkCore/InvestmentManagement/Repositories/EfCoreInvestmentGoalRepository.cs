using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.InvestmentManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreInvestmentGoalRepository 
        : EfCoreRepository<IInvestmentManagementDbContext, InvestmentGoal, Guid>, 
          IInvestmentGoalRepository
    {
        public EfCoreInvestmentGoalRepository(
            IDbContextProvider<IInvestmentManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<InvestmentGoal>> GetUserGoalsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.InvestmentGoals
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
        }

        public async Task<List<InvestmentGoal>> GetActiveGoalsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.InvestmentGoals
                .Where(g => g.UserId == userId && !g.IsAchieved)
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
        }

        public async Task<List<InvestmentGoal>> GetGoalsByPortfolioAsync(Guid portfolioId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.InvestmentGoals
                .Where(g => g.PortfolioId == portfolioId)
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
        }
    }
}
