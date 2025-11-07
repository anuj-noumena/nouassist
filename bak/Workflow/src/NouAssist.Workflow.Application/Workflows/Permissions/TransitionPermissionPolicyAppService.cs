using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Data;
using NouAssist.Workflow.Permissions;
using NouAssist.Workflow.Permissions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Workflow.Workflows.Permissions
{
    [Authorize(WorkflowPermissions.Workflows.Default)]
    public class TransitionPermissionPolicyAppService(ITransitionPermissionPolicyRepository permissionPolicyRepository)
        : ApplicationService, ITransitionPermissionPolicyAppService
    {
        public async Task<TransitionPermissionPolicyDto> GetAsync(Guid id)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
            return ObjectMapper.Map<TransitionPermissionPolicy,
                TransitionPermissionPolicyDto>(policy);
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
            foreach (var role in input.AllowedRoles ? ? new List<string>())
            {
                policy.AddAllowedRole(role);
            }
            // Add allowed users
            foreach (var userId in input.AllowedUserIds ? ? new List<Guid>())
            {
                policy.AddAllowedUser(userId);
            }
            await permissionPolicyRepository.InsertAsync(policy);
            return ObjectMapper.Map<TransitionPermissionPolicy,
                TransitionPermissionPolicyDto>(policy);
        }

        public async Task<TransitionPermissionPolicyDto> UpdateAsync(Guid id, UpdateTransitionPermissionPolicyDto input)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
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
            foreach (var role in input.AllowedRoles ? ? new List<string>())
            {
                policy.AddAllowedRole(role);
            }
            // Update users
            policy.ClearAllowedUsers();
            foreach (var userId in input.AllowedUserIds ? ? new List<Guid>())
            {
                policy.AddAllowedUser(userId);
            }
            await permissionPolicyRepository.UpdateAsync(policy);
            return ObjectMapper.Map<TransitionPermissionPolicy,
                TransitionPermissionPolicyDto>(policy);
        }

        public async Task DeleteAsync(Guid id)
        {
            await permissionPolicyRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<TransitionPermissionPolicyDto>> GetListAsync(
            PagedAndSortedResultRequestDto input)
        {
            var queryable = await permissionPolicyRepository.GetQueryableAsync();
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
            var policyDtos = ObjectMapper.Map<List<TransitionPermissionPolicy>,
                List<TransitionPermissionPolicyDto>>(policies);
            return new PagedResultDto<TransitionPermissionPolicyDto>(
                totalCount,
                policyDtos
            );
        }

        public async Task<TransitionPermissionPolicyDto> GetApplicablePolicyAsync(Guid transitionId,
            Guid? projectId = null)
        {
            var policy = await permissionPolicyRepository.GetApplicablePolicyAsync(transitionId, projectId);
            return policy != null
                ? ObjectMapper.Map<TransitionPermissionPolicy,
                    TransitionPermissionPolicyDto>(policy)
                : null;
        }

        public async Task<List<TransitionPermissionPolicyDto>> GetPoliciesForTransitionAsync(Guid transitionId,
            Guid? projectId = null)
        {
            var policies = await permissionPolicyRepository.GetPoliciesForTransitionAsync(transitionId, projectId);
            return ObjectMapper.Map<List<TransitionPermissionPolicy>,
                List<TransitionPermissionPolicyDto>>(policies);
        }

        public async Task<List<TransitionPermissionPolicyDto>> GetPoliciesByWorkflowAsync(Guid workflowId,
            bool includeInactive = false)
        {
            var policies = await permissionPolicyRepository.GetPoliciesByWorkflowAsync(workflowId, includeInactive);
            return ObjectMapper.Map<List<TransitionPermissionPolicy>,
                List<TransitionPermissionPolicyDto>>(policies);
        }

        public async Task<List<TransitionPermissionPolicyDto>> GetPoliciesByProjectAsync(Guid projectId,
            bool includeInactive = false)
        {
            var policies = await permissionPolicyRepository.GetPoliciesByProjectAsync(projectId, includeInactive);
            return ObjectMapper.Map<List<TransitionPermissionPolicy>,
                List<TransitionPermissionPolicyDto>>(policies);
        }

        public async Task<bool> CanUserPerformTransitionAsync(Guid transitionId, Guid userId, List<string> userRoles,
            Guid? projectId = null)
        {
            var policy = await permissionPolicyRepository.GetApplicablePolicyAsync(transitionId, projectId);
            // No policy = allow all
            if (policy == null)
            {
                return true;
            }

            return policy.CanUserPerform(userId, userRoles);
        }

        public async Task ActivateAsync(Guid id)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
            policy.Activate();
            await permissionPolicyRepository.UpdateAsync(policy);
        }

        public async Task DeactivateAsync(Guid id)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
            policy.Deactivate();
            await permissionPolicyRepository.UpdateAsync(policy);
        }

        public async Task AddAllowedRoleAsync(Guid id, string role)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
            policy.AddAllowedRole(role);
            await permissionPolicyRepository.UpdateAsync(policy);
        }

        public async Task RemoveAllowedRoleAsync(Guid id, string role)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
            policy.RemoveAllowedRole(role);
            await permissionPolicyRepository.UpdateAsync(policy);
        }

        public async Task AddAllowedUserAsync(Guid id, Guid userId)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
            policy.AddAllowedUser(userId);
            await permissionPolicyRepository.UpdateAsync(policy);
        }

        public async Task RemoveAllowedUserAsync(Guid id, Guid userId)
        {
            var policy = await permissionPolicyRepository.GetAsync(id);
            policy.RemoveAllowedUser(userId);
            await permissionPolicyRepository.UpdateAsync(policy);
        }
    }
}