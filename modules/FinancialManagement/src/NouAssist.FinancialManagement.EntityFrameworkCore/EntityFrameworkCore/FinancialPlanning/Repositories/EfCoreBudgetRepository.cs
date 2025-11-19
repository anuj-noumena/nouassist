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
    public class EfCoreBudgetRepository
        : EfCoreRepository<IFinancialManagementDbContext, Budget, Guid>,
          IBudgetRepository
    {
        public EfCoreBudgetRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Budget?> GetWithAllocationsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Budgets
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Budget>> GetBudgetsByUserAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Budgets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.PeriodStart)
                .ToListAsync();
        }

        public async Task<List<Budget>> GetActiveBudgetsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Budgets
                .Where(b => b.UserId == userId && b.Status == BudgetStatus.Active)
                .OrderByDescending(b => b.PeriodStart)
                .ToListAsync();
        }

        public async Task<Budget?> GetCurrentBudgetAsync(Guid userId, DateTime date)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Budgets
                .Where(b => b.UserId == userId
                    && b.PeriodStart <= date
                    && b.PeriodEnd >= date
                    && b.Status == BudgetStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Budget>> GetBudgetsByStatusAsync(Guid userId, BudgetStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Budgets
                .Where(b => b.UserId == userId && b.Status == status)
                .OrderByDescending(b => b.PeriodStart)
                .ToListAsync();
        }

        public async Task<List<Budget>> GetBudgetsByTypeAsync(Guid userId, BudgetType type)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Budgets
                .Where(b => b.UserId == userId && b.Type == type)
                .OrderByDescending(b => b.PeriodStart)
                .ToListAsync();
        }
    }
}
