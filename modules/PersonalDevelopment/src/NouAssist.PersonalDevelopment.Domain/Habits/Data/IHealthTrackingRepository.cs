using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.PersonalDevelopment.Habits.Data
{
    public interface IHealthTrackingRepository : IRepository<HealthTrackingEntry, Guid>
    {
        Task<List<HealthTrackingEntry>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate, DateTime endDate,
            CancellationToken cancellationToken = default
        );
        
        Task<HealthTrackingEntry> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );
    }
}
