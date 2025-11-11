using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkManagement;

[DependsOn(
    typeof(WorkManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class WorkManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(WorkManagementApplicationContractsModule).Assembly,
            WorkManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkManagementHttpApiClientModule>();
        });

    }
}
