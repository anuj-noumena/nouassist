using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace NouAssist.CollaborationHub.TeamCollaboration;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class TeamCollaborationInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TeamCollaborationInstallerModule>();
        });
    }
}
