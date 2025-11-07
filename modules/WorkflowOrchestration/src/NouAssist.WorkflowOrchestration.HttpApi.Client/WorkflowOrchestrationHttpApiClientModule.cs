using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkflowOrchestration;

[DependsOn(
    typeof(WorkflowOrchestrationApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class WorkflowOrchestrationHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(WorkflowOrchestrationApplicationContractsModule).Assembly,
            WorkflowOrchestrationRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkflowOrchestrationHttpApiClientModule>();
        });

    }
}
