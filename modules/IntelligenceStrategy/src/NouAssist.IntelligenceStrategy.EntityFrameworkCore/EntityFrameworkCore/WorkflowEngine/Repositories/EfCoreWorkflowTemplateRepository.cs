using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IWorkflowTemplateRepository
    /// </summary>
    public class EfCoreWorkflowTemplateRepository 
        : EfCoreRepository<IWorkflowEngineDbContext, WorkflowTemplate, Guid>, 
          IWorkflowTemplateRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="EfCoreWorkflowTemplateRepository"/>
        /// </summary>
        /// <param name="dbContextProvider">The DB context provider</param>
        public EfCoreWorkflowTemplateRepository(
            IDbContextProvider<IWorkflowEngineDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <inheritdoc/>
        public async Task<WorkflowTemplate> GetByNameAsync(
            string name,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // No specific details to include for WorkflowTemplate

            return await query
                .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<WorkflowTemplate>> GetByCategoryAsync(
            string category,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // No specific details to include for WorkflowTemplate

            return await query
                .Where(t => t.Category == category)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<WorkflowTemplate>> GetPublicTemplatesAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            // No specific details to include for WorkflowTemplate

            return await query
                .Where(t => t.IsPublic)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByNameAsync(
            string name,
            Guid? excludingId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .WhereIf(excludingId.HasValue, t => t.Id != excludingId)
                .AnyAsync(t => t.Name == name, cancellationToken);
        }
    }
}
