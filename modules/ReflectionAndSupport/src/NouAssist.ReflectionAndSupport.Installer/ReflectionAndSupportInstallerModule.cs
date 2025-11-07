using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ReflectionAndSupportInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ReflectionAndSupportInstallerModule>();
        });
    }
}
