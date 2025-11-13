using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger.Data
{
    public interface ILedgerAccountRepository : IRepository<LedgerAccount, Guid>
    {
        Task<LedgerAccount> GetByAccountNumberAsync(
            string accountNumber, 
            CancellationToken cancellationToken = default);

        Task<List<LedgerAccount>> GetByTypeAsync(
            AccountType type, 
            bool activeOnly = true,
            CancellationToken cancellationToken = default);

        Task<List<LedgerAccount>> GetAllActiveAccountsAsync(
            CancellationToken cancellationToken = default);

        Task<bool> AccountNumberExistsAsync(
            string accountNumber,
            CancellationToken cancellationToken = default);
    }
}
