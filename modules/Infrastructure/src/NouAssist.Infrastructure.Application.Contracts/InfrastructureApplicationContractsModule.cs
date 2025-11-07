using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(InfrastructureDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class InfrastructureApplicationContractsModule : AbpModule
{

}
