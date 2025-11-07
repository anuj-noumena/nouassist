using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity;

[DependsOn(
    typeof(CoreProductivityApplicationModule),
    typeof(CoreProductivityDomainTestModule)
    )]
public class CoreProductivityApplicationTestModule : AbpModule
{

}
