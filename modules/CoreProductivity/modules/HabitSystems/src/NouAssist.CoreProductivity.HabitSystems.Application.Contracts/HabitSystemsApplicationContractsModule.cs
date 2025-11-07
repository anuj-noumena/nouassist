using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CoreProductivity.HabitSystems;

[DependsOn(
    typeof(HabitSystemsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class HabitSystemsApplicationContractsModule : AbpModule
{

}
