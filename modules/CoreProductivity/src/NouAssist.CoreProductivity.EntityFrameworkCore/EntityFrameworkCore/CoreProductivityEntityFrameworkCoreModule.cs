using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.EntityFrameworkCore;

[DependsOn(
    typeof(CoreProductivityDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CoreProductivityEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CoreProductivityDbContext>(options =>
        {
            options.AddDefaultRepositories<ICoreProductivityDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
