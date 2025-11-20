using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Repositories
{
    public class EfCoreKnowledgeAssetRepository : 
        EfCoreRepository<KnowledgeInnovationDbContext, KnowledgeAsset, Guid>, 
        IKnowledgeAssetRepository
    {
        public EfCoreKnowledgeAssetRepository(
            IDbContextProvider<KnowledgeInnovationDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<List<KnowledgeAsset>> GetListAsync(
            Specification<KnowledgeAsset> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<KnowledgeAsset> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<KnowledgeAsset> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<List<KnowledgeAsset>> GetByTypeAsync(
            KnowledgeAssetType assetType, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(a => a.AssetType == assetType)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<KnowledgeAsset>> GetByTagAsync(
            string tag, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(a => a.Tags.Contains(tag))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<KnowledgeAsset>> SearchByTitleAsync(
            string searchText, 
            int maxResults = 10, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return await query
                    .OrderByDescending(a => a.CreationTime)
                    .Take(maxResults)
                    .ToListAsync(cancellationToken);
            }
            
            return await query
                .Where(a => EF.Functions.ILike(a.Title, $"%{searchText}%"))
                .OrderByDescending(a => a.CreationTime)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<KnowledgeAsset>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(a => a.ContentVersions);
        }
    }
}
