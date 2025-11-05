using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Search
{
    [DependsOn(
        typeof(NouAssistSearchDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class NouAssistSearchApplicationContractsModule : AbpModule
    {
    }
}
