using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.Journaling;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class JournalingInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<JournalingInstallerModule>();
        });
    }
}
