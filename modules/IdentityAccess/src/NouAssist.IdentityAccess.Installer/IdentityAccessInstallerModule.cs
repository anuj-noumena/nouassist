using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class IdentityAccessInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityAccessInstallerModule>();
        });
    }
}
