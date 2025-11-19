using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories
{
    public class EfCoreTaskRecurrenceRepository
        : EfCoreRepository<IWorkManagementDbContext, TaskRecurrenceAggregate, Guid>, ITaskRecurrenceRepository
    {
        public EfCoreTaskRecurrenceRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskRecurrenceAggregate> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .FirstOrDefaultAsync(tr => tr.TaskId == taskId, cancellationToken);
        }
    }
}
