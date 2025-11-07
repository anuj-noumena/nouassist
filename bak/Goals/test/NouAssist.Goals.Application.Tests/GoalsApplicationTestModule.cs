using Volo.Abp.Modularity;

namespace NouAssist.Goals;

[DependsOn(
    typeof(GoalsApplicationModule),
    typeof(GoalsDomainTestModule)
    )]
public class GoalsApplicationTestModule : AbpModule
{

}
