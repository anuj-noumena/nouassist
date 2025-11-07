using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Infrastructure.SearchEngine;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class SearchEngineInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SearchEngineInstallerModule>();
        });
    }
}
