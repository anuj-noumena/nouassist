using Microsoft.Extensions.DependencyInjection;
using NouAssist.Notifications.Aggregates;
using NouAssist.Notifications.Data;
using NouAssist.Notifications.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.Notifications.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistNotificationsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class NotificationsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<NotificationsDbContext>(options =>
            {
                options.AddRepository<Notification, EfCoreNotificationRepository>();
                options.AddRepository<NotificationPreference, EfCoreNotificationPreferenceRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
