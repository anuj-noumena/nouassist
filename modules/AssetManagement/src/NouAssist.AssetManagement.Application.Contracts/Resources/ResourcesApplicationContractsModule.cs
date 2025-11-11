using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement.Resources;

[DependsOn(
    typeof(ResourcesDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ResourcesApplicationContractsModule : AbpModule
{

}
