using Microsoft.Extensions.DependencyInjection;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement.Resources.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistResourcesDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class ResourcesEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ResourcesDbContext>(options =>
            {
                options.AddRepository<Resource, EfCoreResourceRepository>();
                options.AddRepository<Book, EfCoreBookRepository>();
                options.AddRepository<InventoryItem, EfCoreInventoryItemRepository>();
                options.AddRepository<StorageBackend, EfCoreStorageBackendRepository>();
                options.AddRepository<BookCollection, EfCoreBookCollectionRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
