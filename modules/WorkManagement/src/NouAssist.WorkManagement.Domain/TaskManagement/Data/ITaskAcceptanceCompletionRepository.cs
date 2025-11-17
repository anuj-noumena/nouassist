using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskAcceptanceCompletionRepository : IRepository<Aggregates.TaskAcceptanceCompletion, Guid>
    {
        Task<Aggregates.TaskAcceptanceCompletion> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        Task<Aggregates.TaskAcceptanceCompletion> GetWithCompletionsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
