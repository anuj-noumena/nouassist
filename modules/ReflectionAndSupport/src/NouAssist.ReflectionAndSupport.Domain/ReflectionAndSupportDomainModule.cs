using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ReflectionAndSupportDomainSharedModule)
)]
public class ReflectionAndSupportDomainModule : AbpModule
{

}
