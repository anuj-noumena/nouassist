using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.HabitSystems;

[DependsOn(
    typeof(HabitSystemsApplicationModule),
    typeof(HabitSystemsDomainTestModule)
    )]
public class HabitSystemsApplicationTestModule : AbpModule
{

}
