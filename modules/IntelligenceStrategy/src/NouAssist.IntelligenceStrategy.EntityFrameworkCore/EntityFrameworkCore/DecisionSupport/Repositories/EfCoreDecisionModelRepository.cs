using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the <see cref="IDecisionModelRepository"/> interface
    /// </summary>
    public class EfCoreDecisionModelRepository 
        : EfCoreRepository<IDecisionSupportDbContext, DecisionModelInstance, Guid>, 
          IDecisionModelRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="EfCoreDecisionModelRepository"/>
        /// </summary>
        public EfCoreDecisionModelRepository(
            IDbContextProvider<IDecisionSupportDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <inheritdoc/>
        public async Task<DecisionModelInstance> GetWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(x => x.Criteria)
                .Include(x => x.Alternatives)
                    .ThenInclude(a => a.Scores)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<DecisionModelInstance>> GetByFinalizedStatusAsync(
            bool isFinalized,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(x => x.IsFinalized == isFinalized)
                .OrderByDescending(x => x.CreationTime)
                .ToListAsync(cancellationToken);
        }
        
        /// <summary>
        /// Gets decision models ordered by specified criteria
        /// </summary>
        public async Task<List<DecisionModelInstance>> GetListAsync(
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .OrderBy(sorting ?? nameof(DecisionModelInstance.CreationTime) + " desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }
        
        /// <summary>
        /// Gets the count of all decision models
        /// </summary>
        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query.CountAsync(cancellationToken);
        }
        
        /// <inheritdoc/>
        public override async Task<IQueryable<DecisionModelInstance>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(x => x.Criteria)
                .Include(x => x.Alternatives)
                    .ThenInclude(a => a.Scores);
        }
    }
}
