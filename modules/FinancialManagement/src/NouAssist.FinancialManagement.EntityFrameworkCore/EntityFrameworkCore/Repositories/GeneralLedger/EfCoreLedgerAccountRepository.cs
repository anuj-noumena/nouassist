using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.GeneralLedger
{
    public class EfCoreLedgerAccountRepository 
        : EfCoreRepository<IFinancialManagementDbContext, LedgerAccount, Guid>,
          ILedgerAccountRepository
    {
        public EfCoreLedgerAccountRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<LedgerAccount> GetByAccountNumberAsync(
            string accountNumber,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(
                    a => a.AccountNumber == accountNumber,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<List<LedgerAccount>> GetByTypeAsync(
            AccountType type,
            bool activeOnly = true,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.Where(a => a.Type == type);

            if (activeOnly)
            {
                query = query.Where(a => a.IsActive);
            }

            return await query
                .OrderBy(a => a.AccountNumber)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<LedgerAccount>> GetAllActiveAccountsAsync(
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(a => a.IsActive)
                .OrderBy(a => a.AccountNumber)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> AccountNumberExistsAsync(
            string accountNumber,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .AnyAsync(
                    a => a.AccountNumber == accountNumber,
                    GetCancellationToken(cancellationToken));
        }
    }
}
