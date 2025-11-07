using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.ReflectionAndSupport.Reminders;

[DependsOn(
    typeof(RemindersDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class RemindersApplicationContractsModule : AbpModule
{

}
