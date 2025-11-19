using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AssetManagement.EntityFrameworkCore.Resources.EntityFrameworkCore.Repositories
{
    public class EfCoreStorageBackendRepository 
        : EfCoreRepository<IAssetManagementDbContext, StorageBackend, Guid>, IStorageBackendRepository
    {
        public EfCoreStorageBackendRepository(
            IDbContextProvider<IAssetManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<StorageBackend> GetDefaultAsync()
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(s => s.IsDefault && s.IsActive && !s.IsDeleted);
        }

        public async Task<StorageBackend> FindByNameAsync(string name)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(s => s.Name == name && !s.IsDeleted);
        }
    }
}
