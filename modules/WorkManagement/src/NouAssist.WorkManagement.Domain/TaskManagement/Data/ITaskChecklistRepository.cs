using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskChecklistRepository : IRepository<TaskChecklistAggregate, Guid>
    {
        Task<TaskChecklistAggregate> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);
    }
}
