using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking;

[DependsOn(
    typeof(GoalTrackingDomainModule),
    typeof(GoalTrackingTestBaseModule)
)]
public class GoalTrackingDomainTestModule : AbpModule
{

}
