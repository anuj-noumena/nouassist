using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Integrations;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class IntegrationsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IntegrationsInstallerModule>();
        });
    }
}
