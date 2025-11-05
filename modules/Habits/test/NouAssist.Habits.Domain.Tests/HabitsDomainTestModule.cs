using Volo.Abp.Modularity;

namespace NouAssist.Habits;

[DependsOn(
    typeof(HabitsDomainModule),
    typeof(HabitsTestBaseModule)
)]
public class HabitsDomainTestModule : AbpModule
{

}
