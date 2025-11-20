using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.StartupEntrepreneurship;

[DependsOn(
    typeof(StartupEntrepreneurshipApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class StartupEntrepreneurshipHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(StartupEntrepreneurshipApplicationContractsModule).Assembly,
            StartupEntrepreneurshipRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StartupEntrepreneurshipHttpApiClientModule>();
        });

    }
}
