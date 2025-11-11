using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AssetManagement.Resources.EntityFrameworkCore.Repositories
{
    public class EfCoreBookCollectionRepository 
        : EfCoreRepository<IResourcesDbContext, BookCollection, Guid>, IBookCollectionRepository
    {
        public EfCoreBookCollectionRepository(
            IDbContextProvider<IResourcesDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<BookCollection>> GetByOwnerAsync(Guid ownerId)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(c => c.OwnerId == ownerId && !c.IsDeleted)
                .OrderBy(c => c.SortOrder ?? 0)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<BookCollection> FindByNameAsync(Guid ownerId, string name)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(c => c.OwnerId == ownerId && c.Name == name && !c.IsDeleted);
        }
    }
}
