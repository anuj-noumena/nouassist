using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskDefinitionRepository : IRepository<TaskDefinitionAggregate, Guid>
    {
        Task<TaskDefinitionAggregate> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);
    }
}
