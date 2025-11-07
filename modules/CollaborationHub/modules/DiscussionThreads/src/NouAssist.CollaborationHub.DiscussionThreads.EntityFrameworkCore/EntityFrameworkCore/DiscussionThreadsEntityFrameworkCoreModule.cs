using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.DiscussionThreads.EntityFrameworkCore;

[DependsOn(
    typeof(DiscussionThreadsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class DiscussionThreadsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<DiscussionThreadsDbContext>(options =>
        {
            options.AddDefaultRepositories<IDiscussionThreadsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
