using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment.Goals;

[DependsOn(
    typeof(GoalsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class GoalsApplicationContractsModule : AbpModule
{

}
