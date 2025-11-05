using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Tasks.Data
{
    public interface ITaskRepository : IRepository<Aggregates.Task, Guid>
    {
        Task<Aggregates.Task> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.Task>> GetListAsync(
            ISpecification<Aggregates.Task> spec,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.Task>> GetRecurringTasksDueForProcessingAsync(
            DateTime currentDate,
            CancellationToken cancellationToken = default);
    }
}
