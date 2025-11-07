using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.EntityFrameworkCore;

[DependsOn(
    typeof(CollaborationHubDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CollaborationHubEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CollaborationHubDbContext>(options =>
        {
            options.AddDefaultRepositories<ICollaborationHubDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
