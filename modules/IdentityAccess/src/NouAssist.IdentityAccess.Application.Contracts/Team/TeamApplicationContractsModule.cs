using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Team;

[DependsOn(
    typeof(TeamDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class TeamApplicationContractsModule : AbpModule
{

}
