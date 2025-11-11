using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Customization;

[DependsOn(
    typeof(CustomizationApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CustomizationHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CustomizationApplicationContractsModule).Assembly,
            CustomizationRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CustomizationHttpApiClientModule>();
        });

    }
}
