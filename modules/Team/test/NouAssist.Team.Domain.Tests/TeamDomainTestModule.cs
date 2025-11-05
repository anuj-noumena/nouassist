using Volo.Abp.Modularity;

namespace NouAssist.Team;

[DependsOn(
    typeof(TeamDomainModule),
    typeof(TeamTestBaseModule)
)]
public class TeamDomainTestModule : AbpModule
{

}
