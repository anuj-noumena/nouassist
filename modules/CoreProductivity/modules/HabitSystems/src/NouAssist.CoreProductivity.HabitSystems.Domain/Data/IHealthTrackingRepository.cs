using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.HabitSystems.Data
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
