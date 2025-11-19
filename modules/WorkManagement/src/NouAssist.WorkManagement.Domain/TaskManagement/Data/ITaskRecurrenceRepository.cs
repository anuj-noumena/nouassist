using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskRecurrenceRepository : IRepository<TaskRecurrenceAggregate, Guid>
    {
        Task<TaskRecurrenceAggregate> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);
    }
}
