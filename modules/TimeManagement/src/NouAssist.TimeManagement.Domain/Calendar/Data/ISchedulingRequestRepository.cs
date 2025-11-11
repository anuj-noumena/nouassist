using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.TimeManagement.Calendar.Data
{
    public interface ISchedulingRequestRepository : IRepository<Aggregates.SchedulingRequest, Guid>
    {
        Task<Aggregates.SchedulingRequest> GetWithSlotsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
