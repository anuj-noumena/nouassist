using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CreditScoreMonitoring.EntityFrameworkCore
{
    [ConnectionStringName(CreditScoreMonitoringDbProperties.ConnectionStringName)]
    public class CreditScoreMonitoringDbContext 
        : AbpDbContext<CreditScoreMonitoringDbContext>, ICreditScoreMonitoringDbContext
    {
        public DbSet<CreditReport> CreditReports { get; set; }
        public DbSet<CreditDispute> CreditDisputes { get; set; }

        public CreditScoreMonitoringDbContext(DbContextOptions<CreditScoreMonitoringDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCreditScoreMonitoring(this);
        }
    }
}
