using Volo.Abp.Modularity;

namespace NouAssist.Team;

[DependsOn(
    typeof(TeamApplicationModule),
    typeof(TeamDomainTestModule)
    )]
public class TeamApplicationTestModule : AbpModule
{

}
