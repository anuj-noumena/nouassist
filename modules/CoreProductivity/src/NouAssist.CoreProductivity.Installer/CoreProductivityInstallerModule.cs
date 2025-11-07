using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CoreProductivity;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CoreProductivityInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CoreProductivityInstallerModule>();
        });
    }
}
