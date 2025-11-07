using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.HabitSystems.Data
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
