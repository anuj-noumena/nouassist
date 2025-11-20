using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Repositories
{
    public class EfCoreLearningResourceRepository : 
        EfCoreRepository<KnowledgeInnovationDbContext, LearningResource, Guid>, 
        ILearningResourceRepository
    {
        public EfCoreLearningResourceRepository(
            IDbContextProvider<KnowledgeInnovationDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<List<LearningResource>> GetListAsync(
            Specification<LearningResource> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<LearningResource> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<LearningResource> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<List<LearningResource>> GetPublishedAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.IsPublished)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningResource>> GetFeaturedAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.IsPublished && r.IsFeatured)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningResource>> GetByTypeAsync(
            ResourceType resourceType, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.ResourceType == resourceType)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningResource>> GetCompletedByUserAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            return await query
                .Where(r => r.UserProgressRecords.Any(
                    p => p.UserId == userId && p.Status == CompletionStatus.Completed))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningResource>> GetInProgressByUserAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            return await query
                .Where(r => r.UserProgressRecords.Any(
                    p => p.UserId == userId && p.Status == CompletionStatus.InProgress))
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<LearningResource>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(r => r.UserProgressRecords);
        }
    }
}
