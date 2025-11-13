using Microsoft.Extensions.DependencyInjection;
using NouAssist.Collaboration.Comments.Aggregates;
using NouAssist.Collaboration.Comments.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Collaboration.Comments.EntityFrameworkCore
{
    [DependsOn(
        typeof(CommentsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class CommentsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CommentsDbContext>(options =>
            {
                options.AddRepository<Comment, EfCoreCommentRepository>();
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
