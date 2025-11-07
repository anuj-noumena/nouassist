using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

[DependsOn(
    typeof(ApprovalWorkflowsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ApprovalWorkflowsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ApprovalWorkflowsApplicationContractsModule).Assembly,
            ApprovalWorkflowsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ApprovalWorkflowsHttpApiClientModule>();
        });

    }
}
