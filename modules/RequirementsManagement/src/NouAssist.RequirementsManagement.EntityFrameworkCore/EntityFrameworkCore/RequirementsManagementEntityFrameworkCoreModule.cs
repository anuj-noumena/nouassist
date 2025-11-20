using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.EntityFrameworkCore;

[DependsOn(
    typeof(RequirementsManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class RequirementsManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<RequirementsManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IRequirementsManagementDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
