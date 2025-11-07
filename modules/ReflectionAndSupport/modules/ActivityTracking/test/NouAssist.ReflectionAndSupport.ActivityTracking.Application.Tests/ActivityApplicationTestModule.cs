using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(ActivityTrackingApplicationModule),
    typeof(ActivityTrackingDomainTestModule)
    )]
public class ActivityTrackingApplicationTestModule : AbpModule
{

}
