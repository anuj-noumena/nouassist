using Volo.Abp.Modularity;

namespace NouAssist.Activity;

[DependsOn(
    typeof(ActivityDomainModule),
    typeof(ActivityTestBaseModule)
)]
public class ActivityDomainTestModule : AbpModule
{

}
