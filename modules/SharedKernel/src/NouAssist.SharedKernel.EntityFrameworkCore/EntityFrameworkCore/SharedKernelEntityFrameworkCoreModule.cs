using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.SharedKernel.EntityFrameworkCore;

[DependsOn(
    typeof(SharedKernelDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class SharedKernelEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SharedKernelDbContext>(options =>
        {
            options.AddDefaultRepositories<ISharedKernelDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
