using Volo.Abp.Modularity;

namespace NouAssist.Reminders;

[DependsOn(
    typeof(RemindersDomainModule),
    typeof(RemindersTestBaseModule)
)]
public class RemindersDomainTestModule : AbpModule
{

}
