using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.SharedKernel.Enums;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories
{
    public class EfCoreTaskInstanceRepository
        : EfCoreRepository<IWorkManagementDbContext, TaskInstance, Guid>, ITaskInstanceRepository
    {
        private readonly IDbContextProvider<IWorkManagementDbContext> _dbContextProvider;

        public EfCoreTaskInstanceRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<TaskInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // TaskInstance no longer has navigation properties for details
            // Those are in separate aggregates (TaskChecklistAggregate, TaskFlowControlAggregate, etc.)
            return await query
                .FirstOrDefaultAsync(ti => ti.Id == id, cancellationToken);
        }

        public async Task<List<TaskInstance>> GetListAsync(
            ISpecification<TaskInstance> spec,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // includeDetails parameter is now ignored since TaskInstance has no navigation properties
            // Application layer should query separate aggregates if it needs details

            return await query
                .Where(spec.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetOverdueTasksAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default)
        {
            // Schedule is now in TaskInstanceWorkAggregate, so we need to join
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            return await (from ti in taskInstanceQuery
                         join work in dbContext.TaskInstanceWork on ti.Id equals work.TaskInstanceId
                         where EF.Property<DateTime?>(work, "Schedule_ActualDueDate") != null &&
                               EF.Property<DateTime?>(work, "Schedule_ActualDueDate") < asOfDate &&
                               EF.Property<DateTime?>(work, "Schedule_CompletedAt") == null
                         orderby EF.Property<DateTime?>(work, "Schedule_ActualDueDate")
                         select ti)
                         .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetTaskInstancesByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            // ProjectId is now in TaskOrganizationAggregate, so we need to join
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            return await (from ti in taskInstanceQuery
                         join org in dbContext.TaskOrganizations on ti.Id equals org.TaskInstanceId
                         where org.ProjectId == projectId
                         orderby ti.CreationTime descending
                         select ti)
                         .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetTaskInstanceCountByStatusAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            // ProjectId is now in TaskOrganizationAggregate, so we need to join
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            var result = await (from ti in taskInstanceQuery
                               join org in dbContext.TaskOrganizations on ti.Id equals org.TaskInstanceId
                               where org.ProjectId == projectId
                               group ti by ti.WorkflowStatusId into g
                               select new { StatusId = g.Key, Count = g.Count() })
                               .ToListAsync(cancellationToken);

            return result.ToDictionary(x => x.StatusId, x => x.Count);
        }

        public override async Task<IQueryable<TaskInstance>> WithDetailsAsync()
        {
            // No navigation properties to include anymore
            return await GetQueryableAsync();
        }

        public async Task<List<TaskInstance>> GetTasksByAreaAsync(
            Guid areaId,
            CancellationToken cancellationToken = default)
        {
            // AreaId is now in TaskOrganizationAggregate, so we need to join
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            return await (from ti in taskInstanceQuery
                         join org in dbContext.TaskOrganizations on ti.Id equals org.TaskInstanceId
                         where org.AreaId == areaId && !ti.IsDeleted
                         orderby ti.CreationTime descending
                         select ti)
                         .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetTasksByContextAsync(
            Guid contextId,
            CancellationToken cancellationToken = default)
        {
            // ContextId is now in TaskOrganizationAggregate, so we need to join
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            return await (from ti in taskInstanceQuery
                         join org in dbContext.TaskOrganizations on ti.Id equals org.TaskInstanceId
                         where org.ContextId == contextId && !ti.IsDeleted
                         orderby ti.CreationTime descending
                         select ti)
                         .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetNextActionsAsync(
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            CancellationToken cancellationToken = default)
        {
            // IsNextAction and IsCompleted are in TaskInstanceWorkAggregate
            // ProjectId, AreaId, ContextId are in TaskOrganizationAggregate
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            var query = from ti in taskInstanceQuery
                       join work in dbContext.TaskInstanceWork on ti.Id equals work.TaskInstanceId
                       where EF.Property<bool>(work, "WorkTracking_IsNextAction") &&
                             !EF.Property<bool>(work, "WorkTracking_HasCompleted") &&
                             !ti.IsDeleted
                       select new { TaskInstance = ti, Work = work };

            // Apply optional filters
            if (projectId.HasValue || areaId.HasValue || contextId.HasValue)
            {
                query = from item in query
                       join org in dbContext.TaskOrganizations on item.TaskInstance.Id equals org.TaskInstanceId
                       where (!projectId.HasValue || org.ProjectId == projectId.Value) &&
                             (!areaId.HasValue || org.AreaId == areaId.Value) &&
                             (!contextId.HasValue || org.ContextId == contextId.Value)
                       select item;
            }

            return await query
                .Select(item => item.TaskInstance)
                .OrderBy(ti => ti.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetOverdueTasksFilteredAsync(
            Guid? projectId = null,
            Guid? areaId = null,
            CancellationToken cancellationToken = default)
        {
            // Schedule is in TaskInstanceWorkAggregate
            // ProjectId, AreaId are in TaskOrganizationAggregate
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            var query = from ti in taskInstanceQuery
                       join work in dbContext.TaskInstanceWork on ti.Id equals work.TaskInstanceId
                       where EF.Property<DateTime?>(work, "Schedule_ActualDueDate") != null &&
                             EF.Property<DateTime?>(work, "Schedule_ActualDueDate") < DateTime.UtcNow &&
                             EF.Property<DateTime?>(work, "Schedule_CompletedAt") == null &&
                             !ti.IsDeleted
                       select new { TaskInstance = ti, Work = work };

            // Apply optional filters
            if (projectId.HasValue || areaId.HasValue)
            {
                query = from item in query
                       join org in dbContext.TaskOrganizations on item.TaskInstance.Id equals org.TaskInstanceId
                       where (!projectId.HasValue || org.ProjectId == projectId.Value) &&
                             (!areaId.HasValue || org.AreaId == areaId.Value)
                       select item;
            }

            return await query
                .Select(item => item.TaskInstance)
                .OrderBy(item => EF.Property<DateTime?>(item.Work, "Schedule_ActualDueDate"))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetFilteredListAsync(
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            Guid? assigneeId = null,
            Guid? ownerId = null,
            Guid? workflowStatusId = null,
            bool? isBlocked = null,
            bool? isNextAction = null,
            bool? isOverdue = null,
            EisenhowerQuadrant? quadrant = null,
            DateTime? dueDateFrom = null,
            DateTime? dueDateTo = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var taskInstanceQuery = await GetQueryableAsync();

            // Start with base TaskInstance query
            var query = taskInstanceQuery.Where(ti => !ti.IsDeleted);

            // Apply filters on TaskInstance itself
            if (assigneeId.HasValue)
            {
                query = query.Where(ti => ti.AssigneeId == assigneeId.Value);
            }

            if (ownerId.HasValue)
            {
                query = query.Where(ti => ti.OwnerId == ownerId.Value);
            }

            if (workflowStatusId.HasValue)
            {
                query = query.Where(ti => ti.WorkflowStatusId == workflowStatusId.Value);
            }

            // Build a complex query that joins with related aggregates as needed
            var needsOrgJoin = projectId.HasValue || areaId.HasValue || contextId.HasValue;
            var needsWorkJoin = isNextAction.HasValue || isOverdue.HasValue || quadrant.HasValue ||
                               dueDateFrom.HasValue || dueDateTo.HasValue;
            var needsFlowJoin = isBlocked.HasValue;

            IQueryable<TaskInstance> finalQuery = query;

            // Join with TaskOrganization if needed
            if (needsOrgJoin)
            {
                finalQuery = from ti in finalQuery
                            join org in dbContext.TaskOrganizations on ti.Id equals org.TaskInstanceId
                            where (!projectId.HasValue || org.ProjectId == projectId.Value) &&
                                  (!areaId.HasValue || org.AreaId == areaId.Value) &&
                                  (!contextId.HasValue || org.ContextId == contextId.Value)
                            select ti;
            }

            // Join with TaskInstanceWork if needed
            if (needsWorkJoin)
            {
                var workQuery = from ti in finalQuery
                               join work in dbContext.TaskInstanceWork on ti.Id equals work.TaskInstanceId
                               select new { TaskInstance = ti, Work = work };

                if (isNextAction.HasValue)
                {
                    workQuery = workQuery.Where(item =>
                        EF.Property<bool>(item.Work, "WorkTracking_IsNextAction") == isNextAction.Value);
                }

                if (isOverdue.HasValue && isOverdue.Value)
                {
                    workQuery = workQuery.Where(item =>
                        EF.Property<DateTime?>(item.Work, "Schedule_ActualDueDate") != null &&
                        EF.Property<DateTime?>(item.Work, "Schedule_ActualDueDate") < DateTime.UtcNow &&
                        EF.Property<DateTime?>(item.Work, "Schedule_CompletedAt") == null);
                }

                if (quadrant.HasValue)
                {
                    workQuery = workQuery.Where(item =>
                        EF.Property<int>(item.Work, "Priority_Quadrant") == (int)quadrant.Value);
                }

                if (dueDateFrom.HasValue)
                {
                    workQuery = workQuery.Where(item =>
                        EF.Property<DateTime?>(item.Work, "Schedule_ActualDueDate") != null &&
                        EF.Property<DateTime?>(item.Work, "Schedule_ActualDueDate") >= dueDateFrom.Value);
                }

                if (dueDateTo.HasValue)
                {
                    workQuery = workQuery.Where(item =>
                        EF.Property<DateTime?>(item.Work, "Schedule_ActualDueDate") != null &&
                        EF.Property<DateTime?>(item.Work, "Schedule_ActualDueDate") <= dueDateTo.Value);
                }

                finalQuery = workQuery.Select(item => item.TaskInstance);
            }

            // Join with TaskFlowControl if needed
            if (needsFlowJoin)
            {
                finalQuery = from ti in finalQuery
                            join flow in dbContext.TaskFlowControls on ti.Id equals flow.TaskInstanceId
                            where flow.IsBlocked == isBlocked.Value
                            select ti;
            }

            return await finalQuery
                .OrderByDescending(ti => ti.CreationTime)
                .ToListAsync(cancellationToken);
        }
    }
}