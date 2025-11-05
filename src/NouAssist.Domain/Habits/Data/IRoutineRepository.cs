using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Habits.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Habits.Data
{
    public interface IRoutineRepository : IRepository<Routine, Guid>
    {
        Task<Routine> GetWithStepsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<Routine>> GetActiveRoutinesAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default
        );
    }
}
