using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(NouAssistActivityTrackingDomainSharedModule)
    )]
    public class NouAssistActivityTrackingDomainModule : AbpModule
    {
    }
}
