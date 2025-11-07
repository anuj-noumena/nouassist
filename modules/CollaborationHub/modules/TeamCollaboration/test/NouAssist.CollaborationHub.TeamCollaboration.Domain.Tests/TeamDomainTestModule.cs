using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.TeamCollaboration;

[DependsOn(
    typeof(TeamCollaborationDomainModule),
    typeof(TeamCollaborationTestBaseModule)
)]
public class TeamCollaborationDomainTestModule : AbpModule
{

}
