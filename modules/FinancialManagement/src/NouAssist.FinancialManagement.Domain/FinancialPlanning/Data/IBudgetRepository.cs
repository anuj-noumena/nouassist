using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface IBudgetRepository : IRepository<Budget, Guid>
    {
        Task<Budget?> GetWithAllocationsAsync(Guid id);
        Task<List<Budget>> GetBudgetsByUserAsync(Guid userId);
        Task<List<Budget>> GetActiveBudgetsAsync(Guid userId);
        Task<Budget?> GetCurrentBudgetAsync(Guid userId, DateTime date);
        Task<List<Budget>> GetBudgetsByStatusAsync(Guid userId, BudgetStatus status);
        Task<List<Budget>> GetBudgetsByTypeAsync(Guid userId, BudgetType type);
    }
}
