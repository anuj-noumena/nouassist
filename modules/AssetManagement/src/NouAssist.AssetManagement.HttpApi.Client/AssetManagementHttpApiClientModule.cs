using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AssetManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AssetManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AssetManagementApplicationContractsModule).Assembly,
            AssetManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AssetManagementHttpApiClientModule>();
        });

    }
}
