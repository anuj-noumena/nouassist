using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
    }
}