using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.Team;

[DependsOn(
    typeof(TeamDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class TeamApplicationContractsModule : AbpModule
{

}
