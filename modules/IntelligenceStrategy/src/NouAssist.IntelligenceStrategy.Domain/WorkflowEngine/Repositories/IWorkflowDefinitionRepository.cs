using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories
{
    /// <summary>
    /// Repository interface for workflow definitions
    /// </summary>
    public interface IWorkflowDefinitionRepository : IRepository<WorkflowDefinition, Guid>
    {
        /// <summary>
        /// Finds workflow definitions matching the given specification
        /// </summary>
        /// <param name="specification">The specification to match</param>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of matching workflow definitions</returns>
        Task<List<WorkflowDefinition>> GetListAsync(
            Specification<WorkflowDefinition> specification,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts workflow definitions matching the given specification
        /// </summary>
        /// <param name="specification">The specification to match</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The count of matching workflow definitions</returns>
        Task<int> CountAsync(
            Specification<WorkflowDefinition> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a workflow definition by name
        /// </summary>
        /// <param name="name">The name of the workflow definition</param>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The workflow definition with the given name</returns>
        Task<WorkflowDefinition> GetByNameAsync(
            string name,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a workflow definition with the given name exists
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <param name="excludingId">Optional ID to exclude from the check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a workflow definition with the given name exists</returns>
        Task<bool> ExistsByNameAsync(
            string name,
            Guid? excludingId = null,
            CancellationToken cancellationToken = default);
    }
}
