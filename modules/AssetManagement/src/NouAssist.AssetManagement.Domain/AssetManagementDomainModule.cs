using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AssetManagementDomainSharedModule)
)]
public class AssetManagementDomainModule : AbpModule
{

}
