using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.DebtManagement.Data
{
    public interface IDebtPayoffPlanRepository : IRepository<DebtPayoffPlan, Guid>
    {
        Task<DebtPayoffPlan?> GetWithDebtsAsync(Guid id);
        Task<List<DebtPayoffPlan>> GetActiveUserPlansAsync(Guid userId);
    }
}
