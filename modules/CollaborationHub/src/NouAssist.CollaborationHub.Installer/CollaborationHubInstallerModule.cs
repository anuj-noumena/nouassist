using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CollaborationHubInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CollaborationHubInstallerModule>();
        });
    }
}
