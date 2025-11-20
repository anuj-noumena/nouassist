using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.EntityFrameworkCore;

[DependsOn(
    typeof(StartupEntrepreneurshipDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class StartupEntrepreneurshipEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<StartupEntrepreneurshipDbContext>(options =>
        {
            options.AddDefaultRepositories<IStartupEntrepreneurshipDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
