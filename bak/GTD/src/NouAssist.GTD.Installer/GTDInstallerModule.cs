using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.GTD;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class GTDInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GTDInstallerModule>();
        });
    }
}
