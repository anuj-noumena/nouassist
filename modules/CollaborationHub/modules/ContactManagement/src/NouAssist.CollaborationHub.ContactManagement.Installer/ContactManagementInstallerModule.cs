using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.ContactManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ContactManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ContactManagementInstallerModule>();
        });
    }
}
