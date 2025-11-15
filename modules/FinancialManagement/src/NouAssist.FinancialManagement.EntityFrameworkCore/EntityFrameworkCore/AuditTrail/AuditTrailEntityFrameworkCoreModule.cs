using Microsoft.Extensions.DependencyInjection;
using NouAssist.AuditTrail.Aggregates;
using NouAssist.AuditTrail.Data;
using NouAssist.AuditTrail.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.AuditTrail.EntityFrameworkCore
{
    [DependsOn(
        typeof(AuditTrailDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class AuditTrailEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AuditTrailDbContext>(options =>
            {
                options.AddRepository<AuditEntry, EfCoreAuditEntryRepository>();
                
                // Configure other DbContext options if needed
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
