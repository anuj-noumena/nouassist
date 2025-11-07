using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Goals;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(GoalsDomainSharedModule)
)]
public class GoalsDomainModule : AbpModule
{

}
