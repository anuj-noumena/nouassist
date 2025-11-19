using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories
{
    public class EfCoreTaskInstanceRepository 
        : EfCoreRepository<IWorkManagementDbContext, TaskInstance, Guid>, ITaskInstanceRepository
    {
        public EfCoreTaskInstanceRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(ti => ti.ChecklistItems)
                .Include(ti => ti.CriterionCompletions)
                .Include(ti => ti.Dependencies)
                .FirstOrDefaultAsync(ti => ti.Id == id, cancellationToken);
        }

        public async Task<List<TaskInstance>> GetListAsync(
            ISpecification<TaskInstance> spec,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query
                    .Include(ti => ti.ChecklistItems)
                    .Include(ti => ti.CriterionCompletions)
                    .Include(ti => ti.Dependencies);
            }

            return await query
                .Where(spec.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetOverdueTasksAsync(
            DateTime asOfDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(ti => ti.Schedule.ActualDueDate.HasValue &&
                           ti.Schedule.ActualDueDate.Value < asOfDate &&
                           !ti.Schedule.IsCompleted &&
                           !ti.IsDeleted)
                .OrderBy(ti => ti.Schedule.ActualDueDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskInstance>> GetTaskInstancesByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(ti => ti.ProjectId == projectId && !ti.IsDeleted)
                .OrderByDescending(ti => ti.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, int>> GetTaskInstanceCountByStatusAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            var result = await query
                .Where(ti => ti.ProjectId == projectId && !ti.IsDeleted)
                .GroupBy(ti => ti.WorkflowStatusId)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            return result.ToDictionary(x => x.StatusId, x => x.Count);
        }

        public override async Task<IQueryable<TaskInstance>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(ti => ti.ChecklistItems)
                .Include(ti => ti.CriterionCompletions)
                .Include(ti => ti.Dependencies);
        }
    }
}