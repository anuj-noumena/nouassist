using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories
{
    public class EfCoreTaskRepository
        : EfCoreRepository<IWorkManagementDbContext, TaskAggregate, Guid>, ITaskRepository
    {
        public EfCoreTaskRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskAggregate> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // TaskAggregate no longer has navigation properties for details
            // Those are in separate aggregates (TaskDefinitionAggregate, etc.)
            return await query
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TaskAggregate>> GetListAsync(
            ISpecification<TaskAggregate> spec,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(spec.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskAggregate>> GetRecurringTasksDueForProcessingAsync(
            DateTime currentDate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // To find recurring tasks, we need to check if RecurrenceId is not null
            // The actual recurrence details are in TaskRecurrenceAggregate
            return await query
                .Where(t => t.RecurrenceId != null &&
                           t.IsActive &&
                           !t.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<TaskAggregate>> WithDetailsAsync()
        {
            // No navigation properties to include anymore
            return await GetQueryableAsync();
        }
    }
}