using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using NouAssist.CoreProductivity.TimeManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreTimeEntryRepository 
        : EfCoreRepository<ICalendarDbContext, TimeEntry, Guid>, ITimeEntryRepository
    {
        public EfCoreTimeEntryRepository(
            IDbContextProvider<ICalendarDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TimeEntry> GetRunningEntryAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(te => te.OwnerId == ownerId && te.EndedAt == null)
                .OrderByDescending(te => te.StartedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TimeEntry>> GetEntriesInRangeAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(te => te.OwnerId == ownerId &&
                            te.StartedAt >= startDate &&
                            te.StartedAt < endDate)
                .OrderBy(te => te.StartedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
