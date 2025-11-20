using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Integrations;

[DependsOn(
    typeof(IntegrationsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class IntegrationsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(IntegrationsApplicationContractsModule).Assembly,
            IntegrationsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IntegrationsHttpApiClientModule>();
        });

    }
}
