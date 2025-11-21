using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Repositories
{
    public class EfCoreDecisionRecordRepository : 
        EfCoreRepository<KnowledgeInnovationDbContext, DecisionRecord, Guid>, 
        IDecisionRecordRepository
    {
        public EfCoreDecisionRecordRepository(
            IDbContextProvider<KnowledgeInnovationDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<List<DecisionRecord>> GetListAsync(
            Specification<DecisionRecord> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<DecisionRecord> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<DecisionRecord> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<List<DecisionRecord>> GetByStatusAsync(
            DecisionStatus status, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(d => d.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DecisionRecord>> GetRecentDecisionsAsync(
            DateTime sinceDate, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(d => d.DecisionDate >= sinceDate)
                .OrderByDescending(d => d.DecisionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DecisionRecord>> GetByTagAsync(
            string tag, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(d => d.Tags.Contains(tag))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DecisionRecord>> GetImplementableDecisionsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(d => d.Status == DecisionStatus.Made)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<DecisionRecord>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(d => d.Context)
                .Include(d => d.Alternatives)
                .Include(d => d.Outcome);
        }
    }
}
