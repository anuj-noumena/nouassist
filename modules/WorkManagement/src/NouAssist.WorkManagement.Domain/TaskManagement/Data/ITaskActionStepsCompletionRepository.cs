using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskActionStepsCompletionRepository : IRepository<Aggregates.TaskActionStepsCompletion, Guid>
    {
        Task<Aggregates.TaskActionStepsCompletion> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        Task<Aggregates.TaskActionStepsCompletion> GetWithCompletionsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
