using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;
using Aggregates_Task = NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Task = NouAssist.WorkManagement.TaskManagement.Aggregates.Task;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories
{
    public class EfCoreTaskRepository 
        : EfCoreRepository<ITasksDbContext, Aggregates_Task, Guid>, ITaskRepository
    {
        public EfCoreTaskRepository(
            IDbContextProvider<ITasksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Task> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(t => t.ActionSteps)
                .Include(t => t.AcceptanceCriteria)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<Task>> GetListAsync(
            ISpecification<Task> spec,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Task>> GetRecurringTasksDueForProcessingAsync(
            DateTime currentDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(t => t.ActionSteps)
                .Include(t => t.AcceptanceCriteria)
                .Where(t => t.IsRecurring && 
                           t.IsActive && 
                           !t.IsDeleted &&
                           t.RecurrenceRule != null)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Task>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(t => t.ActionSteps)
                .Include(t => t.AcceptanceCriteria);
        }
    }
}
