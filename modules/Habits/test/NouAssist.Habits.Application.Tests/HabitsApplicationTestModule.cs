using Volo.Abp.Modularity;

namespace NouAssist.Habits;

[DependsOn(
    typeof(HabitsApplicationModule),
    typeof(HabitsDomainTestModule)
    )]
public class HabitsApplicationTestModule : AbpModule
{

}
