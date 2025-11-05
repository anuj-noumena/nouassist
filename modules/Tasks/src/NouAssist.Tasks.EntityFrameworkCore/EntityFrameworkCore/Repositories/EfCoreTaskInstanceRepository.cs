using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Data;
using NouAssist.Tasks.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Tasks.EntityFrameworkCore.Repositories
{
    public class EfCoreTaskInstanceRepository 
        : EfCoreRepository<ITasksDbContext, TaskInstance, Guid>, ITaskInstanceRepository
    {
        public EfCoreTaskInstanceRepository(
            IDbContextProvider<ITasksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(ti => ti.StepCompletions)
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
                    .Include(ti => ti.StepCompletions)
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
                .Where(ti => ti.ActualDueDate.HasValue &&
                           ti.ActualDueDate.Value < asOfDate &&
                           !ti.IsCompleted &&
                           ti.TaskStatus != WorkflowStatusCategory.Cancelled &&
                           !ti.IsDeleted)
                .OrderBy(ti => ti.ActualDueDate)
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
                .Include(ti => ti.StepCompletions)
                .Include(ti => ti.CriterionCompletions)
                .Include(ti => ti.Dependencies);
        }
    }
}
