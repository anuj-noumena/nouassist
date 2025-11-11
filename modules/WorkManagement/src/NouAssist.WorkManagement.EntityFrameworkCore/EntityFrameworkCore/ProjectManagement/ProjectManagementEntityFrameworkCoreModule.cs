using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.ProjectManagement.EntityFrameworkCore;

[DependsOn(
    typeof(ProjectManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ProjectManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ProjectManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IProjectManagementDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
