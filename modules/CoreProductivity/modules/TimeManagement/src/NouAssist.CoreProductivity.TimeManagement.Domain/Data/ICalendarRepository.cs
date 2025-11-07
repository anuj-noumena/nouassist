using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.TimeManagement.Data
{
    public interface ICalendarRepository : IRepository<Aggregates.Calendar, Guid>
    {
        Task<Aggregates.Calendar> GetDefaultCalendarAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<List<Aggregates.Calendar>> GetActiveCalendarsAsync(Guid ownerId, CancellationToken cancellationToken = default);
        Task<Aggregates.Calendar> GetWithSharesAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
