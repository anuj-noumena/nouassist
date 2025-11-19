using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.DebtManagement.Data
{
    public interface IDebtAccountRepository : IRepository<DebtAccount, Guid>
    {
        Task<DebtAccount?> GetWithPaymentsAsync(Guid id);
        Task<List<DebtAccount>> GetActiveDebtsByUserAsync(Guid userId);
        Task<List<DebtAccount>> GetDebtsByTypeAsync(Guid userId, DebtType type);
        Task<decimal> GetTotalDebtBalanceAsync(Guid userId);
    }
}
