using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    public interface ITaskChecklistRepository : IRepository<Aggregates.TaskChecklist, Guid>
    {
        Task<Aggregates.TaskChecklist> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);

        Task<Aggregates.TaskChecklist> GetWithItemsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
