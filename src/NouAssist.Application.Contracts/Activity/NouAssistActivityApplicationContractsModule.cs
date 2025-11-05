using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Activity
{
    [DependsOn(
        typeof(NouAssistActivityDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class NouAssistActivityApplicationContractsModule : AbpModule
    {
    }
}

// ===========================================
// Notifications Modules
// ===========================================
