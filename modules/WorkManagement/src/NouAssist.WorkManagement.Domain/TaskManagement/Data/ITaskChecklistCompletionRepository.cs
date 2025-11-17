using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskChecklistCompletionRepository : IRepository<Aggregates.TaskChecklistCompletion, Guid>
    {
        Task<Aggregates.TaskChecklistCompletion> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        Task<Aggregates.TaskChecklistCompletion> GetWithItemsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
