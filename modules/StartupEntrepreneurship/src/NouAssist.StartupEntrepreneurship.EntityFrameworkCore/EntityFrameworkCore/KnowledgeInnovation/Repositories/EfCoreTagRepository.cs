using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Repositories
{
    public class EfCoreTagRepository : 
        EfCoreRepository<KnowledgeInnovationDbContext, Tag>, 
        ITagRepository
    {
        public EfCoreTagRepository(
            IDbContextProvider<KnowledgeInnovationDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<Tag> FindByNameAsync(
            string name, 
            Guid? tenantId = null, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .FirstOrDefaultAsync(t => t.Name == name && t.TenantId == tenantId, cancellationToken);
        }

        public async Task<List<Tag>> GetAllAsync(
            Guid? tenantId = null, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.TenantId == tenantId)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Tag>> GetPopularTagsAsync(
            int usageCountThreshold = 5, 
            int maxTagsToReturn = 20, 
            Guid? tenantId = null, 
            CancellationToken cancellationToken = default)
        {
            // This is a simplified implementation that just returns all tags
            // In a real implementation, you would join with the entities using the tags 
            // to get actual usage counts
            
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(t => t.TenantId == tenantId)
                .OrderBy(t => t.Name)
                .Take(maxTagsToReturn)
                .ToListAsync(cancellationToken);
        }

        public async Task<Tag> CreateIfNotExistsAsync(
            string name, 
            Guid? tenantId = null, 
            CancellationToken cancellationToken = default)
        {
            var tag = await FindByNameAsync(name, tenantId, cancellationToken);
            
            if (tag == null)
            {
                tag = new Tag(name, tenantId);
                await InsertAsync(tag, autoSave: true, cancellationToken: cancellationToken);
            }
            
            return tag;
        }
    }
}
