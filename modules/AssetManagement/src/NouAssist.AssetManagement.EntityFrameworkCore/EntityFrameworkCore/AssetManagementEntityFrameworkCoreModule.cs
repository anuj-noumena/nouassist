using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AssetManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AssetManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AssetManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IAssetManagementDbContext>(includeAllEntities: true);
            
            /* Add custom repositories here. Example:
            * options.AddRepository<Question, EfCoreQuestionRepository>();
            */
        });
    }
}
