using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.HabitSystems;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(HabitSystemsDomainSharedModule)
)]
public class HabitSystemsDomainModule : AbpModule
{

}
