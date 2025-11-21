using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the <see cref="IStrategicInitiativeRepository"/> interface
    /// </summary>
    public class EfCoreStrategicInitiativeRepository 
        : EfCoreRepository<IDecisionSupportDbContext, StrategicInitiativeProposal, Guid>, 
          IStrategicInitiativeRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="EfCoreStrategicInitiativeRepository"/>
        /// </summary>
        public EfCoreStrategicInitiativeRepository(
            IDbContextProvider<IDecisionSupportDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <inheritdoc/>
        public async Task<List<StrategicInitiativeProposal>> GetListAsync(
            Specification<StrategicInitiativeProposal> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<StrategicInitiativeProposal>> GetListWithAnalysesAsync(
            Expression<Func<StrategicInitiativeProposal, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(x => x.SupportingAnalyses)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<StrategicInitiativeProposal> GetWithAnalysesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(x => x.SupportingAnalyses)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        
        /// <summary>
        /// Gets initiatives with specified status and orders them by specified criteria
        /// </summary>
        public async Task<List<StrategicInitiativeProposal>> GetByStatusAsync(
            ProposalStatus status,
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(x => x.Status == status)
                .OrderBy(sorting ?? nameof(StrategicInitiativeProposal.CreationTime) + " desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }
        
        /// <summary>
        /// Gets the count of initiatives with the specified status
        /// </summary>
        public async Task<int> GetCountByStatusAsync(
            ProposalStatus status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(x => x.Status == status)
                .CountAsync(cancellationToken);
        }
    }
}
