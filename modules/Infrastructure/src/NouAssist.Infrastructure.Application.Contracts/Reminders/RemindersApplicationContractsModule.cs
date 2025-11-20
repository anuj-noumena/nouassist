using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Notification.Reminders;

[DependsOn(
    typeof(RemindersDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class RemindersApplicationContractsModule : AbpModule
{

}
