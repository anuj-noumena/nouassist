using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger.Data
{
    public interface IJournalEntryRepository : IRepository<JournalEntryBatch, Guid>
    {
        Task<JournalEntryBatch> GetWithLinesAsync(
            Guid id, 
            CancellationToken cancellationToken = default);

        Task<List<JournalEntryBatch>> GetUnpostedBatchesInPeriodAsync(
            Guid periodId,
            CancellationToken cancellationToken = default);

        Task<List<JournalEntryBatch>> GetByAccountAsync(
            Guid accountId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
    }
}
