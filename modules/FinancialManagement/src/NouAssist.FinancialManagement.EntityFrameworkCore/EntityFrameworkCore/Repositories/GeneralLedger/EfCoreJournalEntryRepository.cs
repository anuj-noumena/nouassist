using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Data;
using NouAssist.FinancialManagement.GeneralLedger.Entities;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore.Repositories.GeneralLedger
{
    public class EfCoreJournalEntryRepository 
        : EfCoreRepository<IFinancialManagementDbContext, JournalEntryBatch, Guid>,
          IJournalEntryRepository
    {
        public EfCoreJournalEntryRepository(
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
                .FirstOrDefaultAsync(
                    b => b.Id == id,
                    GetCancellationToken(cancellationToken));
        }

        public async Task<List<JournalEntryBatch>> GetUnpostedBatchesInPeriodAsync(
            Guid periodId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var period = await dbContext.AccountingPeriods
                .FirstOrDefaultAsync(p => p.Id == periodId, cancellationToken);

            if (period == null)
                return new List<JournalEntryBatch>();

            return await dbContext.JournalEntryBatches
                .Where(b => b.PostingDate >= period.StartDate &&
                           b.PostingDate <= period.EndDate &&
                           b.Status != BatchStatus.Posted)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<JournalEntryBatch>> GetByAccountAsync(
            Guid accountId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = from batch in dbContext.JournalEntryBatches
                        where dbContext.Set<JournalEntryLine>()
                            .Any(l => l.BatchId == batch.Id && l.AccountId == accountId)
                        select batch;

            if (startDate.HasValue)
            {
                query = query.Where(b => b.PostingDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(b => b.PostingDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(b => b.PostingDate)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<JournalEntryBatch>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(b => b.Lines);
        }
    }
}
