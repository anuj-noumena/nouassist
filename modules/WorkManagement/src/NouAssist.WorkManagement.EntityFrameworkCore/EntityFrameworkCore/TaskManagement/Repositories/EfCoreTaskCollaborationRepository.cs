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
    public class EfCoreTaskCollaborationRepository
        : EfCoreRepository<IWorkManagementDbContext, TaskCollaborationAggregate, Guid>, ITaskCollaborationRepository
    {
        public EfCoreTaskCollaborationRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskCollaborationAggregate> GetByTaskInstanceIdAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(tc => tc.Watchers)
                .Include(tc => tc.Comments)
                .Include(tc => tc.Attachments)
                .FirstOrDefaultAsync(tc => tc.TaskInstanceId == taskInstanceId, cancellationToken);
        }
    }
}
