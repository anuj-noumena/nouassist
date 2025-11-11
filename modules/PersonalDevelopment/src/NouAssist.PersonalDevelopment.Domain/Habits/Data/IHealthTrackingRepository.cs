using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.PersonalDevelopment.Habits.Data
{
    public interface IHealthTrackingRepository : IRepository<HealthTrackingEntry, Guid>
    {
        Task<HealthTrackingEntry> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );
    }
}
