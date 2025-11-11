using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AssetManagementDomainModule),
    typeof(AssetManagementTestBaseModule)
)]
public class AssetManagementDomainTestModule : AbpModule
{

}
