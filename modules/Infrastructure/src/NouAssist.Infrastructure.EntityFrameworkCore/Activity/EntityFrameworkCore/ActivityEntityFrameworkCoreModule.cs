using Microsoft.Extensions.DependencyInjection;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Activity.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Activity.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistActivityDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class ActivityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ActivityDbContext>(options =>
            {
                options.AddRepository<ActivityFeed, EfCoreActivityFeedRepository>();
                options.AddRepository<AuditLog, EfCoreAuditLogRepository>();
                options.AddRepository<EntityHistory, EfCoreEntityHistoryRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
