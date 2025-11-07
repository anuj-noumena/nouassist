using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Projects;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ProjectsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProjectsInstallerModule>();
        });
    }
}
