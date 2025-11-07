using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.Resources;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ResourcesInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ResourcesInstallerModule>();
        });
    }
}
