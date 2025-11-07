using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reviews;

[DependsOn(
    typeof(ReviewsDomainModule),
    typeof(ReviewsTestBaseModule)
)]
public class ReviewsDomainTestModule : AbpModule
{

}
