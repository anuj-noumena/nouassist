using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.PersonalDevelopment.Habits.Data
{
    public interface IHabitRepository : IRepository<Habit, Guid>
    {
        Task<Habit> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<Habit>> GetActiveHabitsAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default
        );

        Task<List<Habit>> GetScheduledForDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<Habit>> GetListAsync(
            ISpecification<Habit> spec,
            CancellationToken cancellationToken = default
        );
    }
}
