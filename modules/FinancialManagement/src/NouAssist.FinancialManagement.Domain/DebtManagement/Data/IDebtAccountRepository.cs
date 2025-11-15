using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.DebtManagement.Aggregates;
using NouAssist.DebtManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.DebtManagement.Data
{
    public interface IDebtAccountRepository : IRepository<DebtAccount, Guid>
    {
        Task<DebtAccount?> GetWithPaymentsAsync(Guid id);
        Task<List<DebtAccount>> GetActiveDebtsByUserAsync(Guid userId);
        Task<List<DebtAccount>> GetDebtsByTypeAsync(Guid userId, DebtType type);
        Task<decimal> GetTotalDebtBalanceAsync(Guid userId);
    }
}
