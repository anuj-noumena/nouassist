using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TaskManagement.EntityFrameworkCore;

[DependsOn(
    typeof(TaskManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class TaskManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TaskManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<ITaskManagementDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
