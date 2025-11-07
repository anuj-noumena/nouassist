using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.Resources;

[DependsOn(
    typeof(ResourcesApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ResourcesHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ResourcesApplicationContractsModule).Assembly,
            ResourcesRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ResourcesHttpApiClientModule>();
        });

    }
}
