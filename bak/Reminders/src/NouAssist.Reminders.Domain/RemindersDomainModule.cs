using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Reminders;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(RemindersDomainSharedModule)
)]
public class RemindersDomainModule : AbpModule
{

}
