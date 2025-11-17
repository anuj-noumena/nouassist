using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    /// <summary>
    /// Repository interface for TaskDependencyAggregate
    /// </summary>
    public interface ITaskDependencyAggregateRepository : IRepository<TaskDependencyAggregate, Guid>
    {
        /// <summary>
        /// Gets a dependency aggregate by task instance ID
        /// </summary>
        Task<TaskDependencyAggregate> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all task instances that depend on a given task instance
        /// </summary>
        Task<List<TaskDependencyAggregate>> GetDependentTaskInstancesAsync(
            Guid dependsOnTaskInstanceId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a dependency aggregate exists for a given task instance ID
        /// </summary>
        Task<bool> ExistsByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a circular dependency would be created
        /// </summary>
        Task<bool> WouldCreateCircularDependencyAsync(
            Guid taskInstanceId,
            Guid dependsOnTaskInstanceId,
            CancellationToken cancellationToken = default);
    }
}
