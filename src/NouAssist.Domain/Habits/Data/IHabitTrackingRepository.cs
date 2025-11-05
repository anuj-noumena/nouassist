using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Habits.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Habits.Data
{
    public interface IHabitTrackingRepository : IRepository<HabitTrackingEntry, Guid>
    {
        Task<HabitTrackingEntry> GetByHabitAndDateAsync(
            Guid habitId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<HabitTrackingEntry>> GetHistoryAsync(
            Guid habitId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default
        );

        Task<List<HabitTrackingEntry>> GetCompletedInPeriodAsync(
            Guid habitId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );
    }
}
