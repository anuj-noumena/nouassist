using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ActivityTrackingInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ActivityTrackingInstallerModule>();
        });
    }
}
