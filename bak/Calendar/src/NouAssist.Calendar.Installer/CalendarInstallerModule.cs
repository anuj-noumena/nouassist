using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Calendar;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CalendarInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CalendarInstallerModule>();
        });
    }
}
