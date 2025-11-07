using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Comments;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class CommentsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CommentsInstallerModule>();
        });
    }
}
