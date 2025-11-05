using Volo.Abp.Modularity;

namespace NouAssist.Goals;

[DependsOn(
    typeof(GoalsDomainModule),
    typeof(GoalsTestBaseModule)
)]
public class GoalsDomainTestModule : AbpModule
{

}
