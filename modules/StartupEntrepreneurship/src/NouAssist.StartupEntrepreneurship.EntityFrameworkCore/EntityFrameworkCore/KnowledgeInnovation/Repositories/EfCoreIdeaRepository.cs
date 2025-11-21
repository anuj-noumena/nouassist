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
    public class EfCoreIdeaRepository : 
        EfCoreRepository<KnowledgeInnovationDbContext, Idea, Guid>, 
        IIdeaRepository
    {
        public EfCoreIdeaRepository(
            IDbContextProvider<KnowledgeInnovationDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<List<Idea>> GetListAsync(
            Specification<Idea> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<Idea> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<Idea> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<List<Idea>> GetByStatusAsync(
            IdeaStatus status, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(i => i.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Idea>> GetByCreatorAsync(
            Guid creatorId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(i => i.CreatorId == creatorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Idea>> GetHighPriorityIdeasAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(i => i.Priority >= 8)
                .OrderByDescending(i => i.Priority)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Idea>> GetEvaluableIdeasAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(i => i.Status == IdeaStatus.UnderEvaluation)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Idea>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(i => i.Evaluations);
        }
    }
}
