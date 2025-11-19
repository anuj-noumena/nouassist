using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.GeneralLedger.Repositories
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

        public async Task<AccountingPeriod> GetByDateAsync(
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.AccountingPeriods
                .FirstOrDefaultAsync(
                    p => p.StartDate <= date.Date && p.EndDate >= date.Date,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<AccountingPeriod> GetByFiscalPeriodAsync(
            int fiscalYear,
            int periodNumber,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.AccountingPeriods
                .FirstOrDefaultAsync(
                    p => p.FiscalYear == fiscalYear && p.PeriodNumber == periodNumber,
                    GetCancellationToken(cancellationToken));
        }
    }
}
