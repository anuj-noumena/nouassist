using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Tasks.EntityFrameworkCore;

[DependsOn(
    typeof(TasksDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class TasksEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TasksDbContext>(options =>
        {
            options.AddDefaultRepositories<ITasksDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
