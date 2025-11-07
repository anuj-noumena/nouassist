using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkflowOrchestration;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class WorkflowOrchestrationInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkflowOrchestrationInstallerModule>();
        });
    }
}
