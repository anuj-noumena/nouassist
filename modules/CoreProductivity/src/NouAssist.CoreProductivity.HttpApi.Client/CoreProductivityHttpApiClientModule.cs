using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity;

[DependsOn(
    typeof(CoreProductivityApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class CoreProductivityHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CoreProductivityApplicationContractsModule).Assembly,
            CoreProductivityRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CoreProductivityHttpApiClientModule>();
        });

    }
}
