using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.Resources.EntityFrameworkCore;

[DependsOn(
    typeof(ResourcesDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ResourcesEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ResourcesDbContext>(options =>
        {
            options.AddDefaultRepositories<IResourcesDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
