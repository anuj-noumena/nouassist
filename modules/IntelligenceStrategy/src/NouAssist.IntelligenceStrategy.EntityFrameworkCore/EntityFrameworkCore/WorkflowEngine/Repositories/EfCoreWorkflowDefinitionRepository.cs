using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IWorkflowDefinitionRepository
    /// </summary>
    public class EfCoreWorkflowDefinitionRepository 
        : EfCoreRepository<IWorkflowEngineDbContext, WorkflowDefinition, Guid>, 
          IWorkflowDefinitionRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="EfCoreWorkflowDefinitionRepository"/>
        /// </summary>
        /// <param name="dbContextProvider">The DB context provider</param>
        public EfCoreWorkflowDefinitionRepository(
            IDbContextProvider<IWorkflowEngineDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <inheritdoc/>
        public async Task<List<WorkflowDefinition>> GetListAsync(
            Specification<WorkflowDefinition> specification,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(w => w.Versions)
                             .ThenInclude(v => v.Activities)
                             .Include(w => w.Versions)
                             .ThenInclude(v => v.Conditions);
            }

            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync(
            Specification<WorkflowDefinition> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<WorkflowDefinition> GetByNameAsync(
            string name,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(w => w.Versions)
                             .ThenInclude(v => v.Activities)
                             .Include(w => w.Versions)
                             .ThenInclude(v => v.Conditions);
            }

            return await query
                .FirstOrDefaultAsync(w => w.Name == name, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByNameAsync(
            string name,
            Guid? excludingId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .WhereIf(excludingId.HasValue, w => w.Id != excludingId)
                .AnyAsync(w => w.Name == name, cancellationToken);
        }

        /// <inheritdoc/>
        public override async Task<IQueryable<WorkflowDefinition>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(w => w.Versions)
                .ThenInclude(v => v.Activities)
                .Include(w => w.Versions)
                .ThenInclude(v => v.Conditions);
        }
    }
}
