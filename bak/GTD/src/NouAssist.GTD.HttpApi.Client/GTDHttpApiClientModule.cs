using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.GTD;

[DependsOn(
    typeof(GTDApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class GTDHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(GTDApplicationContractsModule).Assembly,
            GTDRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GTDHttpApiClientModule>();
        });

    }
}
