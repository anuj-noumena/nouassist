using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.Infrastructure.Notifications;

[DependsOn(
    typeof(NotificationsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class NotificationsApplicationContractsModule : AbpModule
{

}
