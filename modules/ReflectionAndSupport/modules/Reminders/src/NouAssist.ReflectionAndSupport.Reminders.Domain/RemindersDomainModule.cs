using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reminders;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(RemindersDomainSharedModule)
)]
public class RemindersDomainModule : AbpModule
{

}
