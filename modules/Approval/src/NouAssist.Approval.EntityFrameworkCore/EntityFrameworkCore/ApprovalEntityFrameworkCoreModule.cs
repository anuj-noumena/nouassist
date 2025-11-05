using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Approval.EntityFrameworkCore;

[DependsOn(
    typeof(ApprovalDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ApprovalEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ApprovalDbContext>(options =>
        {
            options.AddDefaultRepositories<IApprovalDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
