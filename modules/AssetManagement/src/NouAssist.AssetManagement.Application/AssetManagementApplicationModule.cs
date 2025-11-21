using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Mapperly;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AssetManagementDomainModule),
    typeof(AssetManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule)
    )]
public class AssetManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AssetManagementApplicationModule>();
    }
}
