using Microsoft.Extensions.DependencyInjection;
using NouAssist.CollaborationHub.DiscussionThreads.Aggregates;
using NouAssist.CollaborationHub.DiscussionThreads.Data;
using NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore
{
    [DependsOn(
        typeof(DiscussionThreadsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class DiscussionThreadsEntityFrameworkCoreModule : AbpModule
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
