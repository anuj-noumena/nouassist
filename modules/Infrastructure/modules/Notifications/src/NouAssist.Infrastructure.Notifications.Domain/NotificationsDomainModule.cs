using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Notifications;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(NotificationsDomainSharedModule)
)]
public class NotificationsDomainModule : AbpModule
{

}
