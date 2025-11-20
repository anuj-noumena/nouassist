using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.GeneralLedger.Repositories
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
            AccountNumber accountNumber,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LedgerAccounts
                .FirstOrDefaultAsync(
                    a => a.AccountNumber.Number == accountNumber.Number,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<bool> IsAccountNumberUniqueAsync(
            AccountNumber accountNumber,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.LedgerAccounts
                .Where(a => a.AccountNumber.Number == accountNumber.Number);

            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }

            return !await query.AnyAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<LedgerAccount>> GetAllActiveAccountsAsync(CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LedgerAccounts
                .Where(a => a.IsActive)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<LedgerAccount>> GetByTypeAsync(AccountType type, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.LedgerAccounts
                .Where(a => a.Type == type)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<LedgerAccount>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(a => a.AccountNumber);
        }
    }
}
