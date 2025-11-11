using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.FinancialManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class FinancialManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FinancialManagementInstallerModule>();
        });
    }
}
