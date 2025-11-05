using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.GTD.EntityFrameworkCore;

[DependsOn(
    typeof(GTDDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class GTDEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<GTDDbContext>(options =>
        {
            options.AddDefaultRepositories<IGTDDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
