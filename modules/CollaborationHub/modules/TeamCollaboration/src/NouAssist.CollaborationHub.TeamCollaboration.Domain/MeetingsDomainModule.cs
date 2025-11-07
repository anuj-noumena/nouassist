using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.TeamCollaboration;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(TeamCollaborationDomainSharedModule)
)]
public class TeamCollaborationDomainModule : AbpModule
{

}
