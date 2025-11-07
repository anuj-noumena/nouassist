using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(ActivityTrackingDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ActivityTrackingApplicationContractsModule : AbpModule
{

}
