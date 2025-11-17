using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    /// <summary>
    /// Repository interface for TaskInstanceChecklist aggregate
    /// </summary>
    public interface ITaskInstanceChecklistRepository : IRepository<TaskInstanceChecklist, Guid>
    {
        /// <summary>
        /// Gets a checklist by task instance ID
        /// </summary>
        Task<TaskInstanceChecklist> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a checklist with all details (includes all checklist items)
        /// </summary>
        Task<TaskInstanceChecklist> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a checklist exists for a given task instance ID
        /// </summary>
        Task<bool> ExistsByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);
    }
}
