using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Notifications;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(NotificationsDomainSharedModule)
)]
public class NotificationsDomainModule : AbpModule
{

}
