using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskHierarchyRepository : IRepository<TaskHierarchyAggregate, Guid>
    {
        Task<TaskHierarchyAggregate> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);
    }
}
