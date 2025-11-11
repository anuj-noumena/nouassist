using Microsoft.Extensions.DependencyInjection;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Data;
using NouAssist.Resources.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.Resources.EntityFrameworkCore
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
