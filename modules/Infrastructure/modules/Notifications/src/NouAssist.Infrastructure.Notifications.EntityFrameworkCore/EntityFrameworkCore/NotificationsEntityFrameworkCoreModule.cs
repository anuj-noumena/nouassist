using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Notifications.EntityFrameworkCore;

[DependsOn(
    typeof(NotificationsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class NotificationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<NotificationsDbContext>(options =>
        {
            options.AddDefaultRepositories<INotificationsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
