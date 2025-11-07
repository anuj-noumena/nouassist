using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Tasks;

[DependsOn(
    typeof(TasksApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class TasksHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(TasksApplicationContractsModule).Assembly,
            TasksRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TasksHttpApiClientModule>();
        });

    }
}
