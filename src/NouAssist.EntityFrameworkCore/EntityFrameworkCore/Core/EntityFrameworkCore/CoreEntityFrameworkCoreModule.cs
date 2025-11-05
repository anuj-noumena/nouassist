using Microsoft.Extensions.DependencyInjection;
using NouAssist.Core.Aggregates;
using NouAssist.Core.Data;
using NouAssist.Core.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.Core.EntityFrameworkCore
{
    [DependsOn(
        typeof(CoreDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class CoreEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CoreDbContext>(options =>
            {
                options.AddRepository<Tag, EfCoreTagRepository>();
                options.AddRepository<Context, EfCoreContextRepository>();
                options.AddRepository<Area, EfCoreAreaRepository>();
                options.AddRepository<InboxItem, EfCoreInboxItemRepository>();
                options.AddRepository<LogActivity, EfCoreLogActivityRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
