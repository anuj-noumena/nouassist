using Microsoft.Extensions.DependencyInjection;
using NouAssist.Activity.Aggregates;
using NouAssist.Activity.Data;
using NouAssist.Activity.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.Activity.EntityFrameworkCore
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
