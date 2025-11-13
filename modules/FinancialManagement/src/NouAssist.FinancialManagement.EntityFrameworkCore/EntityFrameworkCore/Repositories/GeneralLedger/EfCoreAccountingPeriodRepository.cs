using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.GeneralLedger
{
    public class EfCoreAccountingPeriodRepository 
        : EfCoreRepository<IFinancialManagementDbContext, AccountingPeriod, Guid>,
          IAccountingPeriodRepository
    {
        public EfCoreAccountingPeriodRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<AccountingPeriod> GetPeriodForDateAsync(
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(
                    p => p.StartDate <= date && p.EndDate >= date,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<AccountingPeriod> GetByFiscalPeriodAsync(
            int fiscalYear,
            int periodNumber,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(
                    p => p.FiscalYear == fiscalYear && p.PeriodNumber == periodNumber,
                    GetCancellationToken(cancellationToken));
        }
    }
}
