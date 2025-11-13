using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger.Data
{
    public interface IAccountingPeriodRepository : IRepository<AccountingPeriod, Guid>
    {
        Task<AccountingPeriod> GetPeriodForDateAsync(
            DateTime date,
            CancellationToken cancellationToken = default);

        Task<AccountingPeriod> GetByFiscalPeriodAsync(
            int fiscalYear,
            int periodNumber,
            CancellationToken cancellationToken = default);
    }
}
