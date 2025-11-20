using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.EntityFrameworkCore;

[DependsOn(
    typeof(BusinessOperationsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class BusinessOperationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BusinessOperationsDbContext>(options =>
        {
            options.AddDefaultRepositories<IBusinessOperationsDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
