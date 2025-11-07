using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.TimeManagement.Data
{
    public interface ITimeEntryRepository : IRepository<Aggregates.TimeEntry, Guid>
    {
        Task<Aggregates.TimeEntry> GetRunningEntryAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<List<Aggregates.TimeEntry>> GetEntriesInRangeAsync(
            Guid ownerId, 
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
    }
}
