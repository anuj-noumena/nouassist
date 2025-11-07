using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NouAssist.Workflow.Aggregates;

using NouAssist.Workflow.Data;

using NouAssist.Workflow.Enums;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

using Volo.Abp.EntityFrameworkCore;

 

namespace NouAssist.Workflow.EntityFrameworkCore.Repositories

{

    public class EfCoreTransitionPermissionPolicyRepository

        : EfCoreRepository<IWorkflowDbContext, TransitionPermissionPolicy, Guid>,

          ITransitionPermissionPolicyRepository

    {

        public EfCoreTransitionPermissionPolicyRepository(

            IDbContextProvider<IWorkflowDbContext> dbContextProvider)

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

 

            // Note: This requires JSON querying capabilities in EF Core

            // AllowedUserIds is stored as JSON, so we use Contains on the collection

            return await dbSet

                .Where(p => p.IsActive && p.AllowedUserIds.Contains(userId))

                .OrderByDescending(p => p.Priority)

                .ToListAsync(cancellationToken);

        }

 

        public async Task<List<TransitionPermissionPolicy>> GetPoliciesByRoleAsync(

            string role,

            CancellationToken cancellationToken = default)

        {

            var dbSet = await GetDbSetAsync();

 

            // Note: This requires JSON querying capabilities in EF Core

            // AllowedRoles is stored as JSON, so we use Contains on the collection

            return await dbSet

                .Where(p => p.IsActive && p.AllowedRoles.Contains(role))

                .OrderByDescending(p => p.Priority)

                .ToListAsync(cancellationToken);

        }

    }

}