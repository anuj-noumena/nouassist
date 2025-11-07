using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Infrastructure.SearchEngine;

[DependsOn(
    typeof(SearchEngineApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class SearchEngineHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(SearchEngineApplicationContractsModule).Assembly,
            SearchRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SearchEngineHttpApiClientModule>();
        });

    }
}
