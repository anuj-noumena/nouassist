using Microsoft.Extensions.DependencyInjection;
using NouAssist.Comments.Aggregates;
using NouAssist.Comments.Data;
using NouAssist.Comments.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Comments.EntityFrameworkCore
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
