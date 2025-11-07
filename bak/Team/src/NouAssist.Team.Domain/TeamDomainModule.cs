using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Team;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(TeamDomainSharedModule)
)]
public class TeamDomainModule : AbpModule
{

}
