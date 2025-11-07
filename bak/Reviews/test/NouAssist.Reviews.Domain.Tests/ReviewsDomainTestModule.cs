using Volo.Abp.Modularity;

namespace NouAssist.Reviews;

[DependsOn(
    typeof(ReviewsDomainModule),
    typeof(ReviewsTestBaseModule)
)]
public class ReviewsDomainTestModule : AbpModule
{

}
