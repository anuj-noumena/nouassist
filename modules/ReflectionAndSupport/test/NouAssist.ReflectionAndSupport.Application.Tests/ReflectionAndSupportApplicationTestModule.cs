using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport;

[DependsOn(
    typeof(ReflectionAndSupportApplicationModule),
    typeof(ReflectionAndSupportDomainTestModule)
    )]
public class ReflectionAndSupportApplicationTestModule : AbpModule
{

}
