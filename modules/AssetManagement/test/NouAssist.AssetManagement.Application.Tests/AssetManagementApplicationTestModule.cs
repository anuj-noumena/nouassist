using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AssetManagementApplicationModule),
    typeof(AssetManagementDomainTestModule)
    )]
public class AssetManagementApplicationTestModule : AbpModule
{

}
