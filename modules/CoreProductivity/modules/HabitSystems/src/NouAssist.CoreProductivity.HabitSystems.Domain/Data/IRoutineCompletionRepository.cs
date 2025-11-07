using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using NouAssist.CoreProductivity.HabitSystems.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.HabitSystems.Data
{
    public interface IRoutineCompletionRepository : IRepository<RoutineCompletion, Guid>
    {
        Task<RoutineCompletion> GetByRoutineAndDateAsync(
            Guid routineId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<List<RoutineCompletion>> GetByRoutineInPeriodAsync(
            Guid routineId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<List<RoutineCompletion>> GetByOwnerAndDateAsync(
            Guid ownerId,
            DateTime date,
            CancellationToken cancellationToken = default
        );

        Task<double> GetCompletionRateAsync(
            Guid routineId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default
        );

        Task<List<RoutineCompletion>> GetByStatusAsync(
            Guid ownerId,
            RoutineCompletionStatus status,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default
        );
    }
}
