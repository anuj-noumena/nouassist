using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.DiscussionThreads;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class DiscussionThreadsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<DiscussionThreadsInstallerModule>();
        });
    }
}
