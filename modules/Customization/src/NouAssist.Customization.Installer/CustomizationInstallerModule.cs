using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Customization;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CustomizationInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CustomizationInstallerModule>();
        });
    }
}
