using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskAcceptanceCriteriaRepository : IRepository<Aggregates.TaskAcceptanceCriteria, Guid>
    {
        Task<Aggregates.TaskAcceptanceCriteria> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);

        Task<Aggregates.TaskAcceptanceCriteria> GetWithCriteriaAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
