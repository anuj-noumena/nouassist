using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Workflows.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow.Workflows.Permissions

{

    /// <summary>

    /// Application service interface for managing transition permission policies.

    /// </summary>

    public interface ITransitionPermissionPolicyAppService : IApplicationService

    {

        /// <summary>

        /// Gets a permission policy by ID.

        /// </summary>

        Task<TransitionPermissionPolicyDto> GetAsync(Guid id);

 

        /// <summary>

        /// Creates a new permission policy.

        /// </summary>

        Task<TransitionPermissionPolicyDto> CreateAsync(CreateTransitionPermissionPolicyDto input);

 

        /// <summary>

        /// Updates an existing permission policy.

        /// </summary>

        Task<TransitionPermissionPolicyDto> UpdateAsync(Guid id, UpdateTransitionPermissionPolicyDto input);

 

        /// <summary>

        /// Deletes a permission policy.

        /// </summary>

        Task DeleteAsync(Guid id);

 

        /// <summary>

        /// Gets a paged list of permission policies.

        /// </summary>

        Task<PagedResultDto<TransitionPermissionPolicyDto>> GetListAsync(PagedAndSortedResultRequestDto input);

 

        /// <summary>

        /// Gets the applicable permission policy for a specific transition.

        /// Evaluates in priority order: Project-specific > Global > None.

        /// </summary>

        Task<TransitionPermissionPolicyDto> GetApplicablePolicyAsync(Guid transitionId, Guid? projectId = null);

 

        /// <summary>

        /// Gets all policies for a specific transition.

        /// </summary>

        Task<List<TransitionPermissionPolicyDto>> GetPoliciesForTransitionAsync(Guid transitionId, Guid? projectId = null);

 

        /// <summary>

        /// Gets all policies for a specific workflow.

        /// </summary>

        Task<List<TransitionPermissionPolicyDto>> GetPoliciesByWorkflowAsync(Guid workflowId, bool includeInactive = false);

 

        /// <summary>

        /// Gets all policies for a specific project.

        /// </summary>

        Task<List<TransitionPermissionPolicyDto>> GetPoliciesByProjectAsync(Guid projectId, bool includeInactive = false);

 

        /// <summary>

        /// Checks if a user can perform a specific transition.

        /// </summary>

        Task<bool> CanUserPerformTransitionAsync(Guid transitionId, Guid userId, List<string> userRoles, Guid? projectId = null);

 

        /// <summary>

        /// Activates a permission policy.

        /// </summary>

        Task ActivateAsync(Guid id);

 

        /// <summary>

        /// Deactivates a permission policy.

        /// </summary>

        Task DeactivateAsync(Guid id);

 

        /// <summary>

        /// Adds a role to the allowed roles list.

        /// </summary>

        Task AddAllowedRoleAsync(Guid id, string role);

 

        /// <summary>

        /// Removes a role from the allowed roles list.

        /// </summary>

        Task RemoveAllowedRoleAsync(Guid id, string role);

 

        /// <summary>

        /// Adds a user to the allowed users list.

        /// </summary>

        Task AddAllowedUserAsync(Guid id, Guid userId);

 

        /// <summary>

        /// Removes a user from the allowed users list.

        /// </summary>

        Task RemoveAllowedUserAsync(Guid id, Guid userId);

    }

}