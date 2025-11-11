using Microsoft.Extensions.DependencyInjection;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.Reviews.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.Reviews.EntityFrameworkCore
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
