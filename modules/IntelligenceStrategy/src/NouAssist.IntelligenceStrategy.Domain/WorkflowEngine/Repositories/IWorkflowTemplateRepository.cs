using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories
{
    /// <summary>
    /// Repository interface for workflow templates
    /// </summary>
    public interface IWorkflowTemplateRepository : IRepository<WorkflowTemplate, Guid>
    {
        /// <summary>
        /// Gets a workflow template by name
        /// </summary>
        /// <param name="name">The name of the workflow template</param>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The workflow template with the given name</returns>
        Task<WorkflowTemplate> GetByNameAsync(
            string name,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all workflow templates in a specific category
        /// </summary>
        /// <param name="category">The category</param>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of workflow templates in the given category</returns>
        Task<List<WorkflowTemplate>> GetByCategoryAsync(
            string category,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets public workflow templates accessible to all tenants
        /// </summary>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of public workflow templates</returns>
        Task<List<WorkflowTemplate>> GetPublicTemplatesAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a workflow template with the given name exists
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <param name="excludingId">Optional ID to exclude from the check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a workflow template with the given name exists</returns>
        Task<bool> ExistsByNameAsync(
            string name,
            Guid? excludingId = null,
            CancellationToken cancellationToken = default);
    }
}
