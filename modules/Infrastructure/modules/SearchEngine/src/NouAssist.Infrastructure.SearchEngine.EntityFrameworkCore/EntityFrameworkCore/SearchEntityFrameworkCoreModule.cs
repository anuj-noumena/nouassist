using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.SearchEngine.EntityFrameworkCore;

[DependsOn(
    typeof(SearchEngineDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class SearchEngineEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SearchDbContext>(options =>
        {
            options.AddDefaultRepositories<ISearchDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
