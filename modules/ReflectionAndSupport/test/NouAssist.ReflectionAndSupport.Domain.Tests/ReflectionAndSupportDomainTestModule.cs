using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport;

[DependsOn(
    typeof(ReflectionAndSupportDomainModule),
    typeof(ReflectionAndSupportTestBaseModule)
)]
public class ReflectionAndSupportDomainTestModule : AbpModule
{

}
