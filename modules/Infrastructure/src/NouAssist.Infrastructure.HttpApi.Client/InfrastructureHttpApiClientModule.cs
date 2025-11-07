using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(InfrastructureApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class InfrastructureHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(InfrastructureApplicationContractsModule).Assembly,
            InfrastructureRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<InfrastructureHttpApiClientModule>();
        });

    }
}
