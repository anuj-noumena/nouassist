using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories
{
    public class EfCoreTaskInstanceWorkRepository
        : EfCoreRepository<IWorkManagementDbContext, TaskInstanceWorkAggregate, Guid>, ITaskInstanceWorkRepository
    {
        public EfCoreTaskInstanceWorkRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskInstanceWorkAggregate> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .FirstOrDefaultAsync(tiw => tiw.TaskInstanceId == taskInstanceId, cancellationToken);
        }
    }
}
