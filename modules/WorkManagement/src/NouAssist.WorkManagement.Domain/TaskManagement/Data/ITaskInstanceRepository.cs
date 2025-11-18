using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskInstanceRepository : IRepository<TaskInstance, Guid>
    {
        Task<TaskInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetListAsync(
            ISpecification<TaskInstance> spec,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetOverdueTasksAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default);

        Task<List<TaskInstance>> GetTaskInstancesByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<Dictionary<Guid, int>> GetTaskInstanceCountByStatusAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
    }
}
