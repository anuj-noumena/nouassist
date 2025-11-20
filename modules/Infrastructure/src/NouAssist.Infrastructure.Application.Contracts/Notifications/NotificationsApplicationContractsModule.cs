using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Notification.Notifications;

[DependsOn(
    typeof(NotificationsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class NotificationsApplicationContractsModule : AbpModule
{

}
