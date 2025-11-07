using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Notifications;

[DependsOn(
    typeof(NotificationsDomainModule),
    typeof(NotificationsTestBaseModule)
)]
public class NotificationsDomainTestModule : AbpModule
{

}
