using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity.TimeManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class TimeManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TimeManagementInstallerModule>();
        });
    }
}
