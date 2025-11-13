using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Data;
using NouAssist.FinancialManagement.FinancialPlanning.Entities;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.FinancialPlanning
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

        public async Task<Budget> GetWithAllocationsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Budgets
                .Include(b => b.Allocations)
                .FirstOrDefaultAsync(
                    b => b.Id == id,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<List<Budget>> GetActiveBudgetsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(b => b.UserId == userId && b.Status == BudgetStatus.Active)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Budget> GetBudgetForPeriodAsync(
            Guid userId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(
                    b => b.UserId == userId &&
                         b.PeriodStart <= date &&
                         b.PeriodEnd >= date &&
                         b.Status == BudgetStatus.Active,
                    GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<Budget>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(b => b.Allocations);
        }
    }
}
