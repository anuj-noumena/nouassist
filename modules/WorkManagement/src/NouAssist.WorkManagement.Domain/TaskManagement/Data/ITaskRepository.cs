using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskRepository : IRepository<TaskAggregate, Guid>
    {
        Task<TaskAggregate> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<TaskAggregate>> GetListAsync(
            ISpecification<TaskAggregate> spec,
            CancellationToken cancellationToken = default);

        Task<List<TaskAggregate>> GetRecurringTasksDueForProcessingAsync(
            DateTime currentDate,
            CancellationToken cancellationToken = default);
    }
}
