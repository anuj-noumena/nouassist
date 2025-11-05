using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Goals;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class GoalsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GoalsInstallerModule>();
        });
    }
}
