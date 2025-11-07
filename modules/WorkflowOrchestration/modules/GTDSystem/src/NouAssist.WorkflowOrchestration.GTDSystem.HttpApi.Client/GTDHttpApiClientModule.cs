using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

[DependsOn(
    typeof(GTDSystemApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class GTDSystemHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(GTDSystemApplicationContractsModule).Assembly,
            GTDRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GTDSystemHttpApiClientModule>();
        });

    }
}
