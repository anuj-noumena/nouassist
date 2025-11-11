using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.PersonalDevelopment.Habits.Data
{
    public interface IDailyReflectionRepository : IRepository<DailyReflection, Guid>
    {
        Task<DailyReflection> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<DailyReflection>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<List<DailyReflection>> GetCompletedReflectionsAsync(
            Guid ownerId,
            int take = 10,
            CancellationToken cancellationToken = default
        );

        Task<bool> HasReflectionForDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );
    }
}
