using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reviews;

[DependsOn(
    typeof(ReviewsApplicationModule),
    typeof(ReviewsDomainTestModule)
    )]
public class ReviewsApplicationTestModule : AbpModule
{

}
