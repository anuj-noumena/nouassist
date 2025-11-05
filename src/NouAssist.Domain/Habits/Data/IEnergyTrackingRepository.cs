using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Habits.Data
{
    public interface IEnergyTrackingRepository : IRepository<EnergyTrackingEntry, Guid>
    {
        Task<EnergyTrackingEntry> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<EnergyTrackingEntry>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<double> GetAverageEnergyLevelAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );
    }
}
