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
    public class EfCoreExpenseRepository
        : EfCoreRepository<IFinancialManagementDbContext, Expense, Guid>,
          IExpenseRepository
    {
        public EfCoreExpenseRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Expense?> GetWithAttachmentsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Expenses
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Expense>> GetExpensesByUserAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetExpensesByCategoryAsync(Guid userId, Guid categoryId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Expenses
                .Where(e => e.UserId == userId && e.CategoryId == categoryId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetExpensesByStatusAsync(Guid userId, ExpenseStatus status)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Expenses
                .Where(e => e.UserId == userId && e.Status == status)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetExpensesInPeriodAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Expenses
                .Where(e => e.UserId == userId
                    && e.ExpenseDate >= startDate
                    && e.ExpenseDate <= endDate)
                .OrderBy(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetUnreportedExpensesAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Expenses
                .Where(e => e.UserId == userId
                    && e.Status == ExpenseStatus.Approved)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalExpensesInPeriodAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Expenses
                .Where(e => e.UserId == userId
                    && e.ExpenseDate >= startDate
                    && e.ExpenseDate <= endDate)
                .SumAsync(e => e.Amount.Amount);
        }
    }
}
