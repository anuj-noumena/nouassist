using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Activity;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ActivityInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ActivityInstallerModule>();
        });
    }
}
