using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    /// <summary>
    /// Repository interface for TaskCollaborationAggregate
    /// </summary>
    public interface ITaskCollaborationAggregateRepository : IRepository<TaskCollaborationAggregate, Guid>
    {
        /// <summary>
        /// Gets a collaboration aggregate by task instance ID
        /// </summary>
        Task<TaskCollaborationAggregate> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all task instances that a user is watching
        /// </summary>
        Task<List<TaskCollaborationAggregate>> GetTaskInstancesWatchedByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a collaboration aggregate exists for a given task instance ID
        /// </summary>
        Task<bool> ExistsByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a user is watching a specific task instance
        /// </summary>
        Task<bool> IsUserWatchingAsync(
            Guid taskInstanceId,
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
