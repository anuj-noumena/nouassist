using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Integrations.EntityFrameworkCore;

[DependsOn(
    typeof(IntegrationsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class IntegrationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IntegrationsDbContext>(options =>
        {
            options.AddDefaultRepositories<IIntegrationsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
