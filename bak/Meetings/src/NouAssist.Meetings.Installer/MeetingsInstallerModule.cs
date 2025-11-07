using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Meetings;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class MeetingsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MeetingsInstallerModule>();
        });
    }
}
