using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(BusinessModelManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class BusinessModelManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BusinessModelManagementDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<BusinessModelCanvas, EfCoreBusinessModelCanvasRepository>();
                options.AddRepository<RevenueModel, EfCoreRevenueModelRepository>();
                options.AddRepository<StrategicPivot, EfCoreStrategicPivotRepository>();

                // Enable default repositories for child entities if needed
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
