using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.TimeManagement;

[DependsOn(
    typeof(TimeManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class TimeManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(TimeManagementApplicationContractsModule).Assembly,
            TimeManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TimeManagementHttpApiClientModule>();
        });

    }
}
