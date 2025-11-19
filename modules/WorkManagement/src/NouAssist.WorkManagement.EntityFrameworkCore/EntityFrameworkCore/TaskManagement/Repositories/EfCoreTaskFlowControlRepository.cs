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
    public class EfCoreTaskFlowControlRepository
        : EfCoreRepository<IWorkManagementDbContext, TaskFlowControlAggregate, Guid>, ITaskFlowControlRepository
    {
        public EfCoreTaskFlowControlRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskFlowControlAggregate> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(tfc => tfc.Dependencies)
                .Include(tfc => tfc.BlockingInfo)
                .Include(tfc => tfc.ApprovalRequests)
                .FirstOrDefaultAsync(tfc => tfc.TaskInstanceId == taskInstanceId, cancellationToken);
        }
    }
}
