using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Contacts;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ContactsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ContactsInstallerModule>();
        });
    }
}
