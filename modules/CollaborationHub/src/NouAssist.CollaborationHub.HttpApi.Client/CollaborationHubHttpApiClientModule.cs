using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub;

[DependsOn(
    typeof(CollaborationHubApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CollaborationHubHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CollaborationHubApplicationContractsModule).Assembly,
            CollaborationHubRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CollaborationHubHttpApiClientModule>();
        });

    }
}
