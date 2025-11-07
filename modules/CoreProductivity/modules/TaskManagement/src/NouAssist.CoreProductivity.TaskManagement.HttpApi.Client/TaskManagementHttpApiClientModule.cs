using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity.TaskManagement;

[DependsOn(
    typeof(TaskManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class TaskManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(TaskManagementApplicationContractsModule).Assembly,
            TaskManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TaskManagementHttpApiClientModule>();
        });

    }
}
