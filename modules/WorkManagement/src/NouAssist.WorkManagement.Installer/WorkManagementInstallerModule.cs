using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.WorkManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class WorkManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkManagementInstallerModule>();
        });
    }
}
