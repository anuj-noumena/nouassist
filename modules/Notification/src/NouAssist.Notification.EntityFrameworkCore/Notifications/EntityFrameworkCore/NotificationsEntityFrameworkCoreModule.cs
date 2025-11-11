using Microsoft.Extensions.DependencyInjection;
using NouAssist.Notification.Notifications.Aggregates;
using NouAssist.Notification.Notifications.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Notification.Notifications.EntityFrameworkCore
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
