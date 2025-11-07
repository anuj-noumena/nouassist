using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.EntityFrameworkCore;

[DependsOn(
    typeof(InfrastructureDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class InfrastructureEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<InfrastructureDbContext>(options =>
        {
            options.AddDefaultRepositories<IInfrastructureDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
