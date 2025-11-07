using Volo.Abp.Modularity;

namespace NouAssist.Notifications;

[DependsOn(
    typeof(NotificationsDomainModule),
    typeof(NotificationsTestBaseModule)
)]
public class NotificationsDomainTestModule : AbpModule
{

}
