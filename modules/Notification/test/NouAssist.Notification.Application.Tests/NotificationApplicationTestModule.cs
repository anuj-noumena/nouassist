using Volo.Abp.Modularity;

namespace NouAssist.Notification;

[DependsOn(
    typeof(NotificationApplicationModule),
    typeof(NotificationDomainTestModule)
    )]
public class NotificationApplicationTestModule : AbpModule
{

}
