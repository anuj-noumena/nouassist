using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Approval;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ApprovalInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ApprovalInstallerModule>();
        });
    }
}
