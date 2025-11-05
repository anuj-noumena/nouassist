using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Reviews;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ReviewsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ReviewsInstallerModule>();
        });
    }
}
