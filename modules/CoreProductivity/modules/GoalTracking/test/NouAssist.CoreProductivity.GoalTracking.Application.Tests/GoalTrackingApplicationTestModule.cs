using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking;

[DependsOn(
    typeof(GoalTrackingApplicationModule),
    typeof(GoalTrackingDomainTestModule)
    )]
public class GoalTrackingApplicationTestModule : AbpModule
{

}
