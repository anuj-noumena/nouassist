using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Journaling.EntityFrameworkCore;

[DependsOn(
    typeof(JournalingDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class JournalingEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<JournalingDbContext>(options =>
        {
            options.AddDefaultRepositories<IJournalingDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
