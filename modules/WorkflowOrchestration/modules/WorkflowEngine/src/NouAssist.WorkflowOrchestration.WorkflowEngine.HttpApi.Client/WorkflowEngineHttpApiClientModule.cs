using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

[DependsOn(
    typeof(WorkflowEngineApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class WorkflowEngineHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(WorkflowEngineApplicationContractsModule).Assembly,
            WorkflowEngineRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkflowEngineHttpApiClientModule>();
        });

    }
}
