using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.GeneralLedger.Repositories
{
    public class EfCoreJournalEntryBatchRepository 
        : EfCoreRepository<IFinancialManagementDbContext, JournalEntryBatch, Guid>, 
          IJournalEntryBatchRepository
    {
        public EfCoreJournalEntryBatchRepository(
            IDbContextProvider<IFinancialManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<JournalEntryBatch> GetWithLinesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.JournalEntryBatches
                .Include(b => b.Lines)
                .FirstOrDefaultAsync(b => b.Id == id, GetCancellationToken(cancellationToken));
        }

        public async Task<List<JournalEntryBatch>> GetByAccountIdAsync(
            Guid accountId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.JournalEntryBatches
                .Include(b => b.Lines)
                .Where(b => b.Lines.Any(l => l.AccountId == accountId));

            if (startDate.HasValue)
            {
                query = query.Where(b => b.PostingDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(b => b.PostingDate.Date <= endDate.Value.Date);
            }

            return await query
                .OrderBy(b => b.PostingDate.Date)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<JournalEntryBatch>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(b => b.Lines);
        }
    }
}
