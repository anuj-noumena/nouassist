using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.TeamCollaboration;

[DependsOn(
    typeof(TeamCollaborationApplicationModule),
    typeof(TeamCollaborationDomainTestModule)
    )]
public class TeamCollaborationApplicationTestModule : AbpModule
{

}
