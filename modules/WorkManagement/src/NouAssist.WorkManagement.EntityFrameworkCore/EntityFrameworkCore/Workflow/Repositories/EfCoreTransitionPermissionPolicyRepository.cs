using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Data;
using NouAssist.WorkManagement.Workflow.Enums;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Workflow.Repositories
{
    public class EfCoreTransitionPermissionPolicyRepository
        : EfCoreRepository<IWorkManagementDbContext, TransitionPermissionPolicy, Guid>,
          ITransitionPermissionPolicyRepository
    {
        public EfCoreTransitionPermissionPolicyRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TransitionPermissionPolicy> GetApplicablePolicyAsync(
            Guid transitionId,
            Guid? projectId = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            var query = dbSet
                .Where(p => p.TransitionId == transitionId && p.IsActive);

            if (projectId.HasValue)
            {
                // First try to find project-specific policy
                var projectPolicy = await query
                    .Where(p => p.Scope == PermissionPolicyScope.Project && p.ProjectId == projectId.Value)
                    .OrderByDescending(p => p.Priority)
                    .FirstOrDefaultAsync(cancellationToken);

                if (projectPolicy != null)
                {
                    return projectPolicy;
                }
            }

            // Fallback to global policy
            return await query
                .Where(p => p.Scope == PermissionPolicyScope.Global && p.ProjectId == null)
                .OrderByDescending(p => p.Priority)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TransitionPermissionPolicy>> GetPoliciesForTransitionAsync(
            Guid transitionId,
            Guid? projectId = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            var query = dbSet
                .Where(p => p.TransitionId == transitionId && p.IsActive);

            if (projectId.HasValue)
            {
                query = query.Where(p =>
                    (p.Scope == PermissionPolicyScope.Global && p.ProjectId == null) ||
                    (p.Scope == PermissionPolicyScope.Project && p.ProjectId == projectId.Value));
            }
            else
            {
                query = query.Where(p => p.Scope == PermissionPolicyScope.Global && p.ProjectId == null);
            }

            return await query
                .OrderByDescending(p => p.Priority)
                .ThenByDescending(p => p.Scope) // Project > Global
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TransitionPermissionPolicy>> GetPoliciesByWorkflowAsync(
            Guid workflowId,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            var query = dbSet.Where(p => p.WorkflowId == workflowId);

            if (!includeInactive)
            {
                query = query.Where(p => p.IsActive);
            }

            return await query
                .OrderByDescending(p => p.Priority)
                .ThenByDescending(p => p.Scope)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TransitionPermissionPolicy>> GetPoliciesByProjectAsync(
            Guid projectId,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            var query = dbSet.Where(p =>
                p.Scope == PermissionPolicyScope.Project &&
                p.ProjectId == projectId);

            if (!includeInactive)
            {
                query = query.Where(p => p.IsActive);
            }

            return await query
                .OrderByDescending(p => p.Priority)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> PolicyExistsAsync(
            Guid transitionId,
            Guid? projectId = null,
            PermissionPolicyScope? scope = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            var query = dbSet.Where(p => p.TransitionId == transitionId && p.IsActive);

            if (scope.HasValue)
            {
                query = query.Where(p => p.Scope == scope.Value);
            }

            if (projectId.HasValue)
            {
                query = query.Where(p => p.ProjectId == projectId.Value);
            }
            else if (scope == PermissionPolicyScope.Global)
            {
                query = query.Where(p => p.ProjectId == null);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<List<TransitionPermissionPolicy>> GetPoliciesByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var userIdCheck = new AllowedUserId(userId);
            try
            {
                
                return await dbSet
                    .Where(p => p.IsActive && p.AllowedUserIds.Contains(userIdCheck))
                    .OrderByDescending(p => p.Priority)
                    .ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException)
            {
                // Fallback to in-memory filtering if JSON querying not supported
                var allPolicies = await dbSet
                    .Where(p => p.IsActive)
                    .ToListAsync(cancellationToken);

                return allPolicies
                    .Where(p => p.AllowedUserIds.Contains(userIdCheck))
                    .OrderByDescending(p => p.Priority)
                    .ToList();
            }
        }

        public async Task<List<TransitionPermissionPolicy>> GetPoliciesByRoleAsync(
            string role,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();

            try
            {
                return await dbSet
                    .Where(p => p.IsActive && p.AllowedRoles.Contains(role))
                    .OrderByDescending(p => p.Priority)
                    .ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException)
            {
                // Fallback to in-memory filtering if JSON querying not supported
                var allPolicies = await dbSet
                    .Where(p => p.IsActive)
                    .ToListAsync(cancellationToken);

                return allPolicies
                    .Where(p => p.AllowedRoles.Contains(role))
                    .OrderByDescending(p => p.Priority)
                    .ToList();
            }
        }
    }
}