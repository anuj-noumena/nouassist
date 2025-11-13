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
    public class EfCoreExpenseRepository 
        : EfCoreRepository<IFinancialManagementDbContext, Expense, Guid>,
          IExpenseRepository
    {
        public EfCoreExpenseRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Expense>> GetByUserAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.Where(e => e.UserId == userId);

            if (startDate.HasValue)
            {
                query = query.Where(e => e.ExpenseDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.ExpenseDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Expense>> GetByCategoryAsync(
            Guid categoryId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(e => e.CategoryId == categoryId &&
                           e.ExpenseDate >= startDate &&
                           e.ExpenseDate <= endDate)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Expense>> GetPendingApprovalsAsync(
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(e => e.Status == ExpenseStatus.Submitted)
                .OrderBy(e => e.ExpenseDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
