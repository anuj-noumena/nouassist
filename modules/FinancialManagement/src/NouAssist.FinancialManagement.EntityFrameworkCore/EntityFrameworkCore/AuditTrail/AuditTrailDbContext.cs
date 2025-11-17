using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AuditTrail.EntityFrameworkCore
{
    [ConnectionStringName(AuditTrailDbProperties.ConnectionStringName)]
    public class AuditTrailDbContext 
        : AbpDbContext<AuditTrailDbContext>, IAuditTrailDbContext
    {
        public DbSet<AuditEntry> AuditEntries { get; set; }

        public AuditTrailDbContext(DbContextOptions<AuditTrailDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditTrail(this);
        }
    }
}
