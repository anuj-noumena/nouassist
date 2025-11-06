using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

using NouAssist.Workflow.Aggregates;

using NouAssist.Workflow.Data;

using NouAssist.Workflow.Permissions.Dtos;

using Volo.Abp;

using Volo.Abp.Application.Dtos;

using Volo.Abp.Application.Services;

using System.Linq.Dynamic.Core;

 

namespace NouAssist.Workflow.Permissions

{

    [Authorize(WorkflowPermissions.Workflows.Default)]

    public class TransitionPermissionPolicyAppService : ApplicationService, ITransitionPermissionPolicyAppService

    {

        private readonly ITransitionPermissionPolicyRepository _permissionPolicyRepository;

 

        public TransitionPermissionPolicyAppService(

            ITransitionPermissionPolicyRepository permissionPolicyRepository)

        {

            _permissionPolicyRepository = permissionPolicyRepository;

        }

 

        public async Task<TransitionPermissionPolicyDto> GetAsync(Guid id)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

            return ObjectMapper.Map<TransitionPermissionPolicy, TransitionPermissionPolicyDto>(policy);

        }

 

        public async Task<TransitionPermissionPolicyDto> CreateAsync(CreateTransitionPermissionPolicyDto input)

        {

            var policy = new TransitionPermissionPolicy(

                GuidGenerator.Create(),

                input.WorkflowId,

                input.TransitionId,

                input.Name,

                input.Scope,

                input.ProjectId,

                input.Priority,

                input.Description

            );

 

            // Add allowed roles

            foreach (var role in input.AllowedRoles ?? new List<string>())

            {

                policy.AddAllowedRole(role);

            }

 

            // Add allowed users

            foreach (var userId in input.AllowedUserIds ?? new List<Guid>())

            {

                policy.AddAllowedUser(userId);

            }

 

            await _permissionPolicyRepository.InsertAsync(policy);

            return ObjectMapper.Map<TransitionPermissionPolicy, TransitionPermissionPolicyDto>(policy);

        }

 

        public async Task<TransitionPermissionPolicyDto> UpdateAsync(Guid id, UpdateTransitionPermissionPolicyDto input)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

 

            policy.UpdateScope(input.Scope, input.ProjectId);

            policy.SetPriority(input.Priority);

 

            if (input.IsActive)

            {

                policy.Activate();

            }

            else

            {

                policy.Deactivate();

            }

 

            // Update roles

            policy.ClearAllowedRoles();

            foreach (var role in input.AllowedRoles ?? new List<string>())

            {

                policy.AddAllowedRole(role);

            }

 

            // Update users

            policy.ClearAllowedUsers();

            foreach (var userId in input.AllowedUserIds ?? new List<Guid>())

            {

                policy.AddAllowedUser(userId);

            }

 

            await _permissionPolicyRepository.UpdateAsync(policy);

            return ObjectMapper.Map<TransitionPermissionPolicy, TransitionPermissionPolicyDto>(policy);

        }

 

        public async Task DeleteAsync(Guid id)

        {

            await _permissionPolicyRepository.DeleteAsync(id);

        }

 

        public async Task<PagedResultDto<TransitionPermissionPolicyDto>> GetListAsync(PagedAndSortedResultRequestDto input)

        {

            var queryable = await _permissionPolicyRepository.GetQueryableAsync();

 

            var totalCount = queryable.Count();

 

            // Apply sorting

            queryable = !string.IsNullOrWhiteSpace(input.Sorting)

                ? queryable.OrderBy(input.Sorting)

                : queryable.OrderByDescending(p => p.Priority).ThenBy(p => p.Name);

 

            // Apply paging

            var policies = queryable

                .Skip(input.SkipCount)

                .Take(input.MaxResultCount)

                .ToList();

 

            var policyDtos = ObjectMapper.Map<List<TransitionPermissionPolicy>, List<TransitionPermissionPolicyDto>>(policies);

 

            return new PagedResultDto<TransitionPermissionPolicyDto>(

                totalCount,

                policyDtos

            );

        }

 

        public async Task<TransitionPermissionPolicyDto> GetApplicablePolicyAsync(Guid transitionId, Guid? projectId = null)

        {

            var policy = await _permissionPolicyRepository.GetApplicablePolicyAsync(transitionId, projectId);

            return policy != null

                ? ObjectMapper.Map<TransitionPermissionPolicy, TransitionPermissionPolicyDto>(policy)

                : null;

        }

 

        public async Task<List<TransitionPermissionPolicyDto>> GetPoliciesForTransitionAsync(Guid transitionId, Guid? projectId = null)

        {

            var policies = await _permissionPolicyRepository.GetPoliciesForTransitionAsync(transitionId, projectId);

            return ObjectMapper.Map<List<TransitionPermissionPolicy>, List<TransitionPermissionPolicyDto>>(policies);

        }

 

        public async Task<List<TransitionPermissionPolicyDto>> GetPoliciesByWorkflowAsync(Guid workflowId, bool includeInactive = false)

        {

            var policies = await _permissionPolicyRepository.GetPoliciesByWorkflowAsync(workflowId, includeInactive);

            return ObjectMapper.Map<List<TransitionPermissionPolicy>, List<TransitionPermissionPolicyDto>>(policies);

        }

 

        public async Task<List<TransitionPermissionPolicyDto>> GetPoliciesByProjectAsync(Guid projectId, bool includeInactive = false)

        {

            var policies = await _permissionPolicyRepository.GetPoliciesByProjectAsync(projectId, includeInactive);

            return ObjectMapper.Map<List<TransitionPermissionPolicy>, List<TransitionPermissionPolicyDto>>(policies);

        }

 

        public async Task<bool> CanUserPerformTransitionAsync(Guid transitionId, Guid userId, List<string> userRoles, Guid? projectId = null)

        {

            var policy = await _permissionPolicyRepository.GetApplicablePolicyAsync(transitionId, projectId);

 

            // No policy = allow all

            if (policy == null)

            {

                return true;

            }

 

            return policy.CanUserPerform(userId, userRoles);

        }

 

        public async Task ActivateAsync(Guid id)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

            policy.Activate();

            await _permissionPolicyRepository.UpdateAsync(policy);

        }

 

        public async Task DeactivateAsync(Guid id)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

            policy.Deactivate();

            await _permissionPolicyRepository.UpdateAsync(policy);

        }

 

        public async Task AddAllowedRoleAsync(Guid id, string role)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

            policy.AddAllowedRole(role);

            await _permissionPolicyRepository.UpdateAsync(policy);

        }

 

        public async Task RemoveAllowedRoleAsync(Guid id, string role)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

            policy.RemoveAllowedRole(role);

            await _permissionPolicyRepository.UpdateAsync(policy);

        }

 

        public async Task AddAllowedUserAsync(Guid id, Guid userId)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

            policy.AddAllowedUser(userId);

            await _permissionPolicyRepository.UpdateAsync(policy);

        }

 

        public async Task RemoveAllowedUserAsync(Guid id, Guid userId)

        {

            var policy = await _permissionPolicyRepository.GetAsync(id);

            policy.RemoveAllowedUser(userId);

            await _permissionPolicyRepository.UpdateAsync(policy);

        }

    }

}