using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.GTD
{
    [DependsOn(
        typeof(GtdDomainModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class GtdApplicationContractsModule : AbpModule
    {
    }
}
