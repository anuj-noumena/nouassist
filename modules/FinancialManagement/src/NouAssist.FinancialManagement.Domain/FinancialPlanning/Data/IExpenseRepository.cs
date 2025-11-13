using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface IExpenseRepository : IRepository<Expense, Guid>
    {
        Task<List<Expense>> GetByUserAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);

        Task<List<Expense>> GetByCategoryAsync(
            Guid categoryId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);

        Task<List<Expense>> GetPendingApprovalsAsync(
            CancellationToken cancellationToken = default);
    }
}
