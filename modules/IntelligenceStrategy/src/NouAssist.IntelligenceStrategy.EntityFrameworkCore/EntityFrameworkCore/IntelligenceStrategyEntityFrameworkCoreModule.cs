using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.IntelligenceStrategy.EntityFrameworkCore;

[DependsOn(
    typeof(IntelligenceStrategyDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class IntelligenceStrategyEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IntelligenceStrategyDbContext>(options =>
        {
            options.AddDefaultRepositories<IIntelligenceStrategyDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
