using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.StartupEntrepreneurship;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class StartupEntrepreneurshipInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StartupEntrepreneurshipInstallerModule>();
        });
    }
}
