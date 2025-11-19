using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Data;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.AssetManagement.EntityFrameworkCore.Resources.EntityFrameworkCore.Repositories
{
    public class EfCoreResourceRepository 
        : EfCoreRepository<IAssetManagementDbContext, Resource, Guid>, IResourceRepository
    {
        public EfCoreResourceRepository(
            IDbContextProvider<IAssetManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Resource> GetWithVersionsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Resources
                .Include(r => r.Versions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Resource> GetWithAclAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Resources
                .Include(r => r.AccessControlList)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Resource>> GetListAsync(
            ISpecification<Resource> spec, 
            int maxResultCount = 50)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .OrderByDescending(r => r.CreationTime)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<List<Resource>> GetByOwnerAsync(Guid ownerId, ResourceKind? kind = null)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(r => r.OwnerId == ownerId && !r.IsDeleted);

            if (kind.HasValue)
            {
                query = query.Where(r => r.Kind == kind.Value);
            }

            return await query
                .OrderByDescending(r => r.CreationTime)
                .ToListAsync();
        }

        public async Task<long> GetTotalSizeByOwnerAsync(Guid ownerId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(r => r.OwnerId == ownerId && r.SizeBytes.HasValue && !r.IsDeleted)
                .SumAsync(r => r.SizeBytes.Value);
        }
    }
}
