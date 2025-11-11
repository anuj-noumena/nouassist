using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Workflow.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.Workflow.EntityFrameworkCore.Repositories
{
    public class EfCoreWorkflowRepository
        : EfCoreRepository<IWorkflowDbContext, Aggregates.Workflow, Guid>, IWorkflowRepository
    {
        public EfCoreWorkflowRepository(
            IDbContextProvider<IWorkflowDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Aggregates.Workflow> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(w => w.Statuses)
                .Include(w => w.Transitions)
                    .ThenInclude(t => t.Actions)
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<Aggregates.Workflow> GetDefaultWorkflowAsync(
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(w => w.Statuses)
                .Include(w => w.Transitions)
                    .ThenInclude(t => t.Actions)
                .FirstOrDefaultAsync(w => w.IsDefault && w.IsActive, cancellationToken);
        }

        public override async Task<IQueryable<Aggregates.Workflow>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(w => w.Statuses)
                .Include(w => w.Transitions)
                    .ThenInclude(t => t.Actions);
        }
    }
}
