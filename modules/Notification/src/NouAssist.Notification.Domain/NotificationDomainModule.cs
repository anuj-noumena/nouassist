using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Notification;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(NotificationDomainSharedModule)
)]
public class NotificationDomainModule : AbpModule
{

}
