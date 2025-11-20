using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger.Data
{
    public interface ILedgerAccountRepository : IRepository<LedgerAccount, Guid>
    {
        Task<LedgerAccount> GetByAccountNumberAsync(
            AccountNumber accountNumber,
            CancellationToken cancellationToken = default);

        Task<bool> IsAccountNumberUniqueAsync(
            AccountNumber accountNumber,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default);

        Task<List<LedgerAccount>> GetAllActiveAccountsAsync(
            CancellationToken cancellationToken = default);

        Task<List<LedgerAccount>> GetByTypeAsync(
            AccountType type,
            CancellationToken cancellationToken = default);
    }
}
