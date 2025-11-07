using Volo.Abp.Modularity;

namespace NouAssist.Notifications;

[DependsOn(
    typeof(NotificationsApplicationModule),
    typeof(NotificationsDomainTestModule)
    )]
public class NotificationsApplicationTestModule : AbpModule
{

}
