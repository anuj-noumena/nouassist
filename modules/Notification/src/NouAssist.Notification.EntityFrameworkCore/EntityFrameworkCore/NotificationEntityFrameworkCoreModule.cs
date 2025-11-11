using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Notification.EntityFrameworkCore;

[DependsOn(
    typeof(NotificationDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class NotificationEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<NotificationDbContext>(options =>
        {
            options.AddDefaultRepositories<INotificationDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
