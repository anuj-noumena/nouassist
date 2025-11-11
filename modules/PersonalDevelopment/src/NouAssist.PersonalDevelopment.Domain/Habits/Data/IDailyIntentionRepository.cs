using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.PersonalDevelopment.Habits.Data
{
    public interface IDailyIntentionRepository : IRepository<DailyIntention, Guid>
    {
        Task<DailyIntention> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<DailyIntention>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<bool> HasIntentionsForDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );
    }
}
