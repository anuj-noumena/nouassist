using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.SharedKernel;

[DependsOn(
    typeof(SharedKernelDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class SharedKernelApplicationContractsModule : AbpModule
{

}
