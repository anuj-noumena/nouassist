using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface IBudgetRepository : IRepository<Budget, Guid>
    {
        Task<Budget> GetWithAllocationsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Budget>> GetActiveBudgetsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<Budget> GetBudgetForPeriodAsync(
            Guid userId,
            DateTime date,
            CancellationToken cancellationToken = default);
    }
}
