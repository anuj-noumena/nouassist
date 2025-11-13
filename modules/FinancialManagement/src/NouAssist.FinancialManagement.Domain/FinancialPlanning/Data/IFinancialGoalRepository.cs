using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.FinancialPlanning.Data
{
    public interface IFinancialGoalRepository : IRepository<FinancialGoal, Guid>
    {
        Task<List<FinancialGoal>> GetByUserAsync(
            Guid userId,
            GoalStatus? status = null,
            CancellationToken cancellationToken = default);

        Task<List<FinancialGoal>> GetGoalsDueBeforeDateAsync(
            DateTime date,
            CancellationToken cancellationToken = default);
    }
}
