using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.IntelligenceStrategy;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class IntelligenceStrategyInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IntelligenceStrategyInstallerModule>();
        });
    }
}
