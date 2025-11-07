using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reminders;

[DependsOn(
    typeof(RemindersApplicationModule),
    typeof(RemindersDomainTestModule)
    )]
public class RemindersApplicationTestModule : AbpModule
{

}
