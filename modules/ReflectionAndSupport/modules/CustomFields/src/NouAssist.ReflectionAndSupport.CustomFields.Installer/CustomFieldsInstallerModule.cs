using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.CustomFields;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CustomFieldsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CustomFieldsInstallerModule>();
        });
    }
}
