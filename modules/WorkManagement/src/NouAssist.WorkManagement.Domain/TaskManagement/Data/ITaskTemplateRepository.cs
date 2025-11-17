using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Data
{
    /// <summary>
    /// Repository interface for TaskTemplate aggregate
    /// </summary>
    public interface ITaskTemplateRepository : IRepository<TaskTemplate, Guid>
    {
        /// <summary>
        /// Gets a task template by task ID
        /// </summary>
        Task<TaskTemplate> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a task template with all details (includes checklist items and acceptance criteria)
        /// </summary>
        Task<TaskTemplate> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a task template exists for a given task ID
        /// </summary>
        Task<bool> ExistsByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);
    }
}
