using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface IExpenseRepository : IRepository<Expense, Guid>
    {
        Task<Expense?> GetWithAttachmentsAsync(Guid id);
        Task<List<Expense>> GetExpensesByUserAsync(Guid userId);
        Task<List<Expense>> GetExpensesByCategoryAsync(Guid userId, Guid categoryId);
        Task<List<Expense>> GetExpensesByStatusAsync(Guid userId, ExpenseStatus status);
        Task<List<Expense>> GetExpensesInPeriodAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<List<Expense>> GetUnreportedExpensesAsync(Guid userId);
        Task<decimal> GetTotalExpensesInPeriodAsync(Guid userId, DateTime startDate, DateTime endDate);
    }
}
