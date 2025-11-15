using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.FinancialManagement.GeneralLedger.Data
{
    public interface IJournalEntryBatchRepository : IRepository<JournalEntryBatch, Guid>
    {
        Task<JournalEntryBatch> GetWithLinesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<JournalEntryBatch>> GetByAccountIdAsync(
            Guid accountId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
    }
}
