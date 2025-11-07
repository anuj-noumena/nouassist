using Volo.Abp.Modularity;

namespace NouAssist.GTD;

[DependsOn(
    typeof(GTDDomainModule),
    typeof(GTDTestBaseModule)
)]
public class GTDDomainTestModule : AbpModule
{

}
