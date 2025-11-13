using Microsoft.Extensions.DependencyInjection;
using NouAssist.WorkManagement.EntityFrameworkCore.Reviews.Repositories;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Reviews
{
    [DependsOn(
        typeof(NouAssistReviewsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ReviewsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ReviewsDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Review, EfCoreReviewRepository>();
                options.AddRepository<ReviewTemplate, EfCoreReviewTemplateRepository>();
                options.AddRepository<ReviewProgress, EfCoreReviewProgressRepository>();
                options.AddRepository<ReviewItem, EfCoreReviewItemRepository>();

                options.AddDefaultRepositories(includeAllEntities: false);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
