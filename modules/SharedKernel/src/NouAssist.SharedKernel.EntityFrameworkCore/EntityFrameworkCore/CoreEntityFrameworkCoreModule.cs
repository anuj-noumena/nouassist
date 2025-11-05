using Microsoft.Extensions.DependencyInjection;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.SharedKernel.EntityFrameworkCore
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
