using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.EntityFrameworkCore;

[DependsOn(
    typeof(WorkManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class WorkManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<WorkManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IWorkManagementDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
