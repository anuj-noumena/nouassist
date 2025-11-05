using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Calendar.Data
{
    public interface IUserAvailabilityRepository : IRepository<Aggregates.UserAvailability, Guid>
    {
        Task<Aggregates.UserAvailability> GetByOwnerIdAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default);
    }
}
