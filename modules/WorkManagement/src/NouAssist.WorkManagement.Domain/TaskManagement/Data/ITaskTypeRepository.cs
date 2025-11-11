using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Tasks.Data
{
    public interface ITaskTypeRepository : IRepository<Aggregates.TaskType, Guid>
    {
        Task<Aggregates.TaskType> GetWithCustomFieldsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<Aggregates.TaskType>> GetActiveTaskTypesAsync(
            CancellationToken cancellationToken = default);

        Task<Aggregates.TaskType> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
