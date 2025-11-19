using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Data;
using NouAssist.TimeManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.Calendar.EntityFrameworkCore.Repositories
{
    public class EfCoreTimeEntryRepository 
        : EfCoreRepository<ITimeManagementDbContext, TimeEntry, Guid>, ITimeEntryRepository
    {
        public EfCoreTimeEntryRepository(
            IDbContextProvider<ITimeManagementDbContext> dbContextProvider)
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
