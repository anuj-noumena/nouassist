using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Goals;

[DependsOn(
    typeof(GoalsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class GoalsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(GoalsApplicationContractsModule).Assembly,
            GoalsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GoalsHttpApiClientModule>();
        });

    }
}
