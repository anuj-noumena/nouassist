using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Habits.Data
{
    public interface IMoodTrackingRepository : IRepository<MoodTrackingEntry, Guid>
    {
        Task<MoodTrackingEntry> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<MoodTrackingEntry>> GetByOwnerInPeriodAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<double> GetAverageMoodRatingAsync(
            Guid ownerId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<List<string>> GetPopularTagsAsync(
            Guid ownerId,
            int topCount = 10,
            CancellationToken cancellationToken = default
        );
    }
}
