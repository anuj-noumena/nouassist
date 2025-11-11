using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AssetManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AssetManagementApplicationContractsModule : AbpModule
{

}
