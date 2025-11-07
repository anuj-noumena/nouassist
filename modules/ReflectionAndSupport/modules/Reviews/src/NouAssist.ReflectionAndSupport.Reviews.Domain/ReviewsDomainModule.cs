using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reviews;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ReviewsDomainSharedModule)
)]
public class ReviewsDomainModule : AbpModule
{

}
