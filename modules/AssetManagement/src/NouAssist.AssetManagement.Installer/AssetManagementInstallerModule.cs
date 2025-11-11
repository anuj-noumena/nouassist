using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.AssetManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AssetManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AssetManagementInstallerModule>();
        });
    }
}
