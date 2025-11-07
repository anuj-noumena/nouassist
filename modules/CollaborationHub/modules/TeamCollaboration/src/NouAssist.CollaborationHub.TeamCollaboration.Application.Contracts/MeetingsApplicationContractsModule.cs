using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CollaborationHub.TeamCollaboration;

[DependsOn(
    typeof(TeamCollaborationDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class TeamCollaborationApplicationContractsModule : AbpModule
{

}
