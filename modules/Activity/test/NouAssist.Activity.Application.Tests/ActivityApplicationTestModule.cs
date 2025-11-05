using Volo.Abp.Modularity;

namespace NouAssist.Activity;

[DependsOn(
    typeof(ActivityApplicationModule),
    typeof(ActivityDomainTestModule)
    )]
public class ActivityApplicationTestModule : AbpModule
{

}
