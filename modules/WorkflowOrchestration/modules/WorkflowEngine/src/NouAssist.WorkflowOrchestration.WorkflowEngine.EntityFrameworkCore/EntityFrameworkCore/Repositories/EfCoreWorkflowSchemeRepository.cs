using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Aggregates;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.EntityFrameworkCore.Repositories
{
    public class EfCoreWorkflowSchemeRepository
        : EfCoreRepository<IWorkflowDbContext, WorkflowScheme, Guid>, IWorkflowSchemeRepository
    {
        public EfCoreWorkflowSchemeRepository(
            IDbContextProvider<IWorkflowDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<WorkflowScheme> GetWithMappingsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(ws => ws.Mappings)
                .FirstOrDefaultAsync(ws => ws.Id == id, cancellationToken);
        }

        public async Task<WorkflowScheme> GetActiveSchemeForProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Include(ws => ws.Mappings)
                .FirstOrDefaultAsync(ws => ws.ProjectId == projectId && ws.IsActive, cancellationToken);
        }

        public override async Task<IQueryable<WorkflowScheme>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(ws => ws.Mappings);
        }
    }
}
