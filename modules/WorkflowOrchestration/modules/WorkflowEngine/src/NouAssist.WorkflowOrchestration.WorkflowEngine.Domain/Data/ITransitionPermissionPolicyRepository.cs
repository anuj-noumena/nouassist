using System;

using System.Collections.Generic;

using System.Threading;

using System.Threading.Tasks;

using NouAssist.WorkflowOrchestration.WorkflowEngine.Aggregates;

using NouAssist.WorkflowOrchestration.WorkflowEngine.Enums;

using Volo.Abp.Domain.Repositories;

 

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Data

{

    /// <summary>

    /// Repository interface for managing transition permission policies.

    /// Provides specialized query methods for permission evaluation.

    /// </summary>

    public interface ITransitionPermissionPolicyRepository : IRepository<TransitionPermissionPolicy, Guid>

    {

        /// <summary>

        /// Gets the applicable permission policy for a specific transition.

        /// Evaluates in priority order: Project-specific > Global > None.

        /// </summary>

        /// <param name="transitionId">The transition to check permissions for.</param>

        /// <param name="projectId">The project context (null for global only).</param>

        /// <param name="cancellationToken">Cancellation token.</param>

        /// <returns>The highest priority applicable policy, or null if none exist.</returns>

        Task<TransitionPermissionPolicy> GetApplicablePolicyAsync(

            Guid transitionId,

            Guid? projectId = null,

            CancellationToken cancellationToken = default);

 

        /// <summary>

        /// Gets all active policies for a specific transition across all scopes.

        /// Ordered by priority (highest first), then scope (Project > Global).

        /// </summary>

        /// <param name="transitionId">The transition ID.</param>

        /// <param name="projectId">Optional project ID to include project-specific policies.</param>

        /// <param name="cancellationToken">Cancellation token.</param>

        /// <returns>List of all active policies.</returns>

        Task<List<TransitionPermissionPolicy>> GetPoliciesForTransitionAsync(

            Guid transitionId,

            Guid? projectId = null,

            CancellationToken cancellationToken = default);

 

        /// <summary>

        /// Gets all policies for a specific workflow.

        /// </summary>

        /// <param name="workflowId">The workflow ID.</param>

        /// <param name="includeInactive">Whether to include inactive policies.</param>

        /// <param name="cancellationToken">Cancellation token.</param>

        /// <returns>List of policies for the workflow.</returns>

        Task<List<TransitionPermissionPolicy>> GetPoliciesByWorkflowAsync(

            Guid workflowId,

            bool includeInactive = false,

            CancellationToken cancellationToken = default);

 

        /// <summary>

        /// Gets all policies for a specific project.

        /// </summary>

        /// <param name="projectId">The project ID.</param>

        /// <param name="includeInactive">Whether to include inactive policies.</param>

        /// <param name="cancellationToken">Cancellation token.</param>

        /// <returns>List of project-specific policies.</returns>

        Task<List<TransitionPermissionPolicy>> GetPoliciesByProjectAsync(

            Guid projectId,

            bool includeInactive = false,

            CancellationToken cancellationToken = default);

 

        /// <summary>

        /// Checks if a policy exists for a specific transition and project combination.

        /// </summary>

        /// <param name="transitionId">The transition ID.</param>

        /// <param name="projectId">Project ID (null for global).</param>

        /// <param name="scope">Optional scope filter.</param>

        /// <param name="cancellationToken">Cancellation token.</param>

        /// <returns>True if a policy exists, false otherwise.</returns>

        Task<bool> PolicyExistsAsync(

            Guid transitionId,

            Guid? projectId = null,

            PermissionPolicyScope? scope = null,

            CancellationToken cancellationToken = default);

 

        /// <summary>

        /// Gets policies that grant permission to a specific user.

        /// Useful for auditing and reporting.

        /// </summary>

        /// <param name="userId">The user ID.</param>

        /// <param name="cancellationToken">Cancellation token.</param>

        /// <returns>List of policies that include this user.</returns>

        Task<List<TransitionPermissionPolicy>> GetPoliciesByUserAsync(

            Guid userId,

            CancellationToken cancellationToken = default);

 

        /// <summary>

        /// Gets policies that grant permission to a specific role.

        /// Useful for auditing and reporting.

        /// </summary>

        /// <param name="role">The role name.</param>

        /// <param name="cancellationToken">Cancellation token.</param>

        /// <returns>List of policies that include this role.</returns>

        Task<List<TransitionPermissionPolicy>> GetPoliciesByRoleAsync(

            string role,

            CancellationToken cancellationToken = default);

    }

}