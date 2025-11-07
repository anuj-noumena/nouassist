using Volo.Abp.Modularity;

namespace NouAssist.Reviews;

[DependsOn(
    typeof(ReviewsApplicationModule),
    typeof(ReviewsDomainTestModule)
    )]
public class ReviewsApplicationTestModule : AbpModule
{

}
