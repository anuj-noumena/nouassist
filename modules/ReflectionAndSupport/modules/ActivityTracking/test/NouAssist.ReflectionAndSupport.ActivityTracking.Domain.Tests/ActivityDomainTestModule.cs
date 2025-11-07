using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(ActivityTrackingDomainModule),
    typeof(ActivityTrackingTestBaseModule)
)]
public class ActivityTrackingDomainTestModule : AbpModule
{

}
