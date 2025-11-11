using Volo.Abp.Modularity;

namespace NouAssist.Notification;

[DependsOn(
    typeof(NotificationDomainModule),
    typeof(NotificationTestBaseModule)
)]
public class NotificationDomainTestModule : AbpModule
{

}
