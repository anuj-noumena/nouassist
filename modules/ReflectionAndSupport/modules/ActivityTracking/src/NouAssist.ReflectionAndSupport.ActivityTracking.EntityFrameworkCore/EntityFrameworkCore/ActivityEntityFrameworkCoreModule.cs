using Microsoft.Extensions.DependencyInjection;
using NouAssist.ReflectionAndSupport.ActivityTracking.Aggregates;
using NouAssist.ReflectionAndSupport.ActivityTracking.Data;
using NouAssist.ReflectionAndSupport.ActivityTracking.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistActivityTrackingDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class ActivityTrackingEntityFrameworkCoreModule : AbpModule
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
