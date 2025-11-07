using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Notifications;

[DependsOn(
    typeof(NotificationsApplicationModule),
    typeof(NotificationsDomainTestModule)
    )]
public class NotificationsApplicationTestModule : AbpModule
{

}
