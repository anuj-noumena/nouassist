using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.RequirementsManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class RequirementsManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RequirementsManagementInstallerModule>();
        });
    }
}
