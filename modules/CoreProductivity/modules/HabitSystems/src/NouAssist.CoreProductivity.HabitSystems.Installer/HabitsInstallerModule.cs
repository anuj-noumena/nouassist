using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity.HabitSystems;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class HabitSystemsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<HabitSystemsInstallerModule>();
        });
    }
}
