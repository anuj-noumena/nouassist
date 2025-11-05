using Volo.Abp.Modularity;

namespace NouAssist.Reminders;

[DependsOn(
    typeof(RemindersApplicationModule),
    typeof(RemindersDomainTestModule)
    )]
public class RemindersApplicationTestModule : AbpModule
{

}
