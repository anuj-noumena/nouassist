using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;
using TaskStatus = NouAssist.IntelligenceStrategy.WorkflowEngine.Enums.TaskStatus;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IWorkflowInstanceRepository
    /// </summary>
    public class EfCoreWorkflowInstanceRepository 
        : EfCoreRepository<IWorkflowEngineDbContext, WorkflowInstance, Guid>, 
          IWorkflowInstanceRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="EfCoreWorkflowInstanceRepository"/>
        /// </summary>
        /// <param name="dbContextProvider">The DB context provider</param>
        public EfCoreWorkflowInstanceRepository(
            IDbContextProvider<IWorkflowEngineDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <inheritdoc/>
        public async Task<List<WorkflowInstance>> GetListAsync(
            Specification<WorkflowInstance> specification,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(i => i.ActivityInstances)
                             .Include(i => i.ExecutionLogs);
            }

            return await DynamicQueryableExtensions.Where(query, specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<WorkflowInstance>> GetByWorkflowDefinitionIdAsync(
            Guid workflowDefinitionId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(i => i.ActivityInstances)
                             .Include(i => i.ExecutionLogs);
            }

            return await query
                .Where(i => i.WorkflowDefinitionId == workflowDefinitionId)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<WorkflowInstance>> GetActiveInstancesAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(i => i.ActivityInstances)
                             .Include(i => i.ExecutionLogs);
            }

            return await query
                .Where(i => i.Status == InstanceStatus.Running || i.Status == InstanceStatus.Waiting)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<ActivityInstance>> GetAssignedTasksAsync(
            AssigneeType assigneeType,
            Guid assigneeId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = dbContext.Set<ActivityInstance>().AsQueryable();

            // Filter by assignee info and status
            return await query
                .Where(a => a.Assignee != null && 
                            a.Assignee.Type == assigneeType && 
                            a.Assignee.Id == assigneeId && 
                            (a.Status == TaskStatus.Assigned || a.Status == TaskStatus.InProgress))
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public override async Task<IQueryable<WorkflowInstance>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(i => i.ActivityInstances)
                .Include(i => i.ExecutionLogs);
        }
    }
}
