using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment.Habits;

[DependsOn(
    typeof(HabitsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class HabitsApplicationContractsModule : AbpModule
{

}
