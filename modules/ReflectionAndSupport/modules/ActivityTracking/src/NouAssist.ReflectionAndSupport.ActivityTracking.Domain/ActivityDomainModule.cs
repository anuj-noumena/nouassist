using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ActivityTrackingDomainSharedModule)
)]
public class ActivityTrackingDomainModule : AbpModule
{

}
