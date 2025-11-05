using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.Tasks;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class TasksInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TasksInstallerModule>();
        });
    }
}
