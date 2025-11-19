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
    public class EfCoreTaskDefinitionRepository
        : EfCoreRepository<IWorkManagementDbContext, TaskDefinitionAggregate, Guid>, ITaskDefinitionRepository
    {
        public EfCoreTaskDefinitionRepository(
            IDbContextProvider<IWorkManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskDefinitionAggregate> GetByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(td => td.ChecklistItems)
                .Include(td => td.AcceptanceCriteria)
                .Include(td => td.ActionSteps)
                .FirstOrDefaultAsync(td => td.TaskId == taskId, cancellationToken);
        }
    }
}
