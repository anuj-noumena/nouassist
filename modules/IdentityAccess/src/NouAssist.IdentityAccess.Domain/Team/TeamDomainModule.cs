using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Team;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(TeamDomainSharedModule)
)]
public class TeamDomainModule : AbpModule
{

}
