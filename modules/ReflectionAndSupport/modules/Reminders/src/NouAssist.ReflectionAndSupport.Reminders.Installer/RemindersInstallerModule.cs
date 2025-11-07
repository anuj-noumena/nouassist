using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.ReflectionAndSupport.Reminders;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class RemindersInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RemindersInstallerModule>();
        });
    }
}
