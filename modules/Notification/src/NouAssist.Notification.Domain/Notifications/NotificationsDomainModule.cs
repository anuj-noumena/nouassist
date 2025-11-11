using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Notification.Notifications;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(NotificationsDomainSharedModule)
)]
public class NotificationsDomainModule : AbpModule
{

}
