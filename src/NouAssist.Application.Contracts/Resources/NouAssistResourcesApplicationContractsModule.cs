using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Resources
{
    [DependsOn(
        typeof(NouAssistResourcesDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class NouAssistResourcesApplicationContractsModule : AbpModule
    {
    }
}

// ===========================================
// Search Modules
// ===========================================
