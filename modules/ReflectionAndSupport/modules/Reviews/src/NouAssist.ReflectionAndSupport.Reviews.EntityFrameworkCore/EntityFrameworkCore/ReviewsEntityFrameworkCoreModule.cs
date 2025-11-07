using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reviews.EntityFrameworkCore;

[DependsOn(
    typeof(ReviewsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ReviewsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ReviewsDbContext>(options =>
        {
            options.AddDefaultRepositories<IReviewsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
