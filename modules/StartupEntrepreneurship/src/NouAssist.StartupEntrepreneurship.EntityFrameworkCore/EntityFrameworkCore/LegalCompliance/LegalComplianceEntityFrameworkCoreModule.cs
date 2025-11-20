using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore.Repositories;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore
{
    [DependsOn(
        typeof(LegalComplianceDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class LegalComplianceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext
            context.Services.AddAbpDbContext<LegalComplianceDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<IntellectualPropertyAsset, EfCoreIntellectualPropertyRepository>();
                options.AddRepository<LegalAgreement, EfCoreLegalAgreementRepository>();
                options.AddRepository<ComplianceObligation, EfCoreComplianceObligationRepository>();
                options.AddRepository<CorporateRecord, EfCoreCorporateRecordRepository>();
            });

            // Configure to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
