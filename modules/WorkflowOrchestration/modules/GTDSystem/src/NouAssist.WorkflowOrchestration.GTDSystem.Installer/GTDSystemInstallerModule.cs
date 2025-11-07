using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class GTDSystemInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GTDSystemInstallerModule>();
        });
    }
}
