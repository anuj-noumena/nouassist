using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity;

[DependsOn(
    typeof(CoreProductivityDomainModule),
    typeof(CoreProductivityTestBaseModule)
)]
public class CoreProductivityDomainTestModule : AbpModule
{

}
