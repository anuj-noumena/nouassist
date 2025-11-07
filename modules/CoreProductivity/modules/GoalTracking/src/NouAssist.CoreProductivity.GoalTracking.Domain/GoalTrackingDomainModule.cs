using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(GoalTrackingDomainSharedModule)
)]
public class GoalTrackingDomainModule : AbpModule
{

}
