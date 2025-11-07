using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class InfrastructureInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<InfrastructureInstallerModule>();
        });
    }
}
