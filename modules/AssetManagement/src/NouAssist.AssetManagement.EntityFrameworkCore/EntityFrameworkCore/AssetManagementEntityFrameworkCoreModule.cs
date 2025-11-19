using Microsoft.Extensions.DependencyInjection;
using NouAssist.AssetManagement.EntityFrameworkCore.Resources.EntityFrameworkCore.Repositories;
using NouAssist.AssetManagement.Resources.Aggregates;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AssetManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AssetManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AssetManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IAssetManagementDbContext>(includeAllEntities: true);
            
              context.Services.AddAbpDbContext<AssetManagementDbContext>(options =>
               {
                   options.AddRepository<Resource, EfCoreResourceRepository>();
                   options.AddRepository<Book, EfCoreBookRepository>();
                   options.AddRepository<InventoryItem, EfCoreInventoryItemRepository>();
                   options.AddRepository<StorageBackend, EfCoreStorageBackendRepository>();
                   options.AddRepository<BookCollection, EfCoreBookCollectionRepository>();
               });
        });
    }
}
