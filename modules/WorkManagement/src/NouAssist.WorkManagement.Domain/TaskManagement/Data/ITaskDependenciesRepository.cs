using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskDependenciesRepository : IRepository<Aggregates.TaskDependencies, Guid>
    {
        Task<Aggregates.TaskDependencies> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        Task<Aggregates.TaskDependencies> GetWithDependenciesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.TaskDependencies>> GetDependentTasksAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);
    }
}
