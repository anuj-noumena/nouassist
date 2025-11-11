using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskInstanceRepository : IRepository<Aggregates.TaskInstance, Guid>
    {
        Task<Aggregates.TaskInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.TaskInstance>> GetListAsync(
            ISpecification<Aggregates.TaskInstance> spec,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.TaskInstance>> GetOverdueTasksAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.TaskInstance>> GetTaskInstancesByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<Dictionary<Guid, int>> GetTaskInstanceCountByStatusAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
    }
}
