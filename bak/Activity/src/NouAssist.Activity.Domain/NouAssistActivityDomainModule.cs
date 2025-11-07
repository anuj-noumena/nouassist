using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Activity
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(NouAssistActivityDomainSharedModule)
    )]
    public class NouAssistActivityDomainModule : AbpModule
    {
    }
}
