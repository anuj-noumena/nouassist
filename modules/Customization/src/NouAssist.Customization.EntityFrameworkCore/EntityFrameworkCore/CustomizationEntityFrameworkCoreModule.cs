using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Customization.EntityFrameworkCore;

[DependsOn(
    typeof(CustomizationDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CustomizationEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CustomizationDbContext>(options =>
        {
            options.AddDefaultRepositories<ICustomizationDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
