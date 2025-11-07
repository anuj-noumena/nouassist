using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Activity;

[DependsOn(
    typeof(ActivityApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ActivityHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ActivityApplicationContractsModule).Assembly,
            ActivityRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ActivityHttpApiClientModule>();
        });

    }
}
