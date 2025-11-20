using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.BusinessOperations;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class BusinessOperationsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<BusinessOperationsInstallerModule>();
        });
    }
}
