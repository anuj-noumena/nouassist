using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore
{
    [DependsOn(
        typeof(QADomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class QAEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<QADbContext>(options =>
            {
                // Register custom repositories here
                options.AddRepository<TestPlan, EfCoreTestPlanRepository>();
                options.AddRepository<TestCycle, EfCoreTestCycleRepository>();
                options.AddRepository<DefectReport, EfCoreDefectReportRepository>();
                options.AddRepository<QualityMetricSnapshot, EfCoreQualityMetricSnapshotRepository>();
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(); // Configure to use PostgreSQL
            });
        }
    }
}
