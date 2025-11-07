using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reminders;

[DependsOn(
    typeof(RemindersDomainModule),
    typeof(RemindersTestBaseModule)
)]
public class RemindersDomainTestModule : AbpModule
{

}
