using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ProjectExecution.Repositories
{
    /// <summary>
    /// Repository for Task aggregate
    /// </summary>
    public interface ITaskRepository : IRepository<Task, Guid>
    {
        /// <summary>
        /// Gets a task with all its child entities
        /// </summary>
        Task<Task> GetWithDetailsAsync(
            Guid id,
            bool includeAssignments = true,
            bool includeDependencies = true,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of tasks by project ID
        /// </summary>
        Task<List<Task>> GetListByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of tasks by phase ID
        /// </summary>
        Task<List<Task>> GetListByPhaseIdAsync(
            Guid phaseId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of tasks assigned to a user
        /// </summary>
        Task<List<Task>> GetListByAssignedUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of tasks by status
        /// </summary>
        Task<List<Task>> GetListByStatusAsync(
            Guid projectId,
            TaskStatus status,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Checks if adding a dependency would create a circular reference
        /// </summary>
        Task<bool> WouldCreateCircularDependencyAsync(
            Guid taskId,
            Guid dependsOnTaskId,
            CancellationToken cancellationToken = default);
    }
}
