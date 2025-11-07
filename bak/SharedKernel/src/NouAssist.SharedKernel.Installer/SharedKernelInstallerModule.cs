using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.SharedKernel;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class SharedKernelInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SharedKernelInstallerModule>();
        });
    }
}
