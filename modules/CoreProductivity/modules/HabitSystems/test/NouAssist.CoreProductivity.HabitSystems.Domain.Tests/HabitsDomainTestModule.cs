using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.HabitSystems;

[DependsOn(
    typeof(HabitSystemsDomainModule),
    typeof(HabitSystemsTestBaseModule)
)]
public class HabitSystemsDomainTestModule : AbpModule
{

}
