using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Users;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class UsersInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<UsersInstallerModule>();
        });
    }
}
