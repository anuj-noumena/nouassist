using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.TimeManagement.Data
{
    public interface ITimeblockRepository : IRepository<Aggregates.Timeblock, Guid>
    {
        Task<Aggregates.Timeblock> GetWithTasksAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Aggregates.Timeblock>> GetTimeblocksInRangeAsync(
            Guid ownerId, 
            DateTime startDate, 
            DateTime endDate, 
            CancellationToken cancellationToken = default);
    }
}
