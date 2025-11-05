using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Habits;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class HabitsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<HabitsInstallerModule>();
        });
    }
}
