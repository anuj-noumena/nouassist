using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.IntelligenceStrategy;

[DependsOn(
    typeof(IntelligenceStrategyApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class IntelligenceStrategyHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(IntelligenceStrategyApplicationContractsModule).Assembly,
            IntelligenceStrategyRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IntelligenceStrategyHttpApiClientModule>();
        });

    }
}
