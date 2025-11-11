using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.PersonalDevelopment;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class PersonalDevelopmentInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PersonalDevelopmentInstallerModule>();
        });
    }
}
